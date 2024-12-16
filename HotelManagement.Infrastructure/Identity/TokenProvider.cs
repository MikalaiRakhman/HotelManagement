using HotelManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelManagement.Infrastructure.Identity
{
	public class TokenProvider
	{
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public TokenProvider(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_configuration = configuration;
			_context = context;
			_userManager = userManager;
		}

		public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Issuer"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(15),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<string> GenerateRefreshToken(Guid applicationUserId, CancellationToken cancellationToken)
		{
			var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

			var refreshToken = new RefreshToken
			{
				Id = Guid.NewGuid(),
				ApplicationUserId = applicationUserId,
				Token = refreshTokenValue,
				Expires = DateTime.UtcNow.AddDays(1),
			};

			_context.RefreshTokens.Add(refreshToken);

			await _context.SaveChangesAsync(cancellationToken);

			return refreshToken.Token;
		}

		public async Task<(string newJwtToken, string newRefreshToken)> RefreshTokens (string refreshToken, CancellationToken cancellationToken)
		{
			var storedToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

			if (storedToken == null || storedToken.Expires < DateTime.UtcNow) 
			{
				throw new UnauthorizedAccessException("Invalid refresh token or an old one");
			}

			var applicationUser = await _userManager.Users.FirstOrDefaultAsync(au => au.Id == storedToken.ApplicationUserId);

			Guard.AgainstNull(applicationUser, nameof(applicationUser));

			var roles = await _context.UserRoles
				.Where(ur => ur.UserId == applicationUser.Id)
				.Join(_context.Roles,
						ur => ur.RoleId,
						role => role.Id,
						(ur, role) => role.Name)
				.ToListAsync(cancellationToken);

			var newJwtToken = GenerateJwtToken(applicationUser, roles);
			var newRefreshToken = await GenerateRefreshToken(applicationUser.Id, cancellationToken);

			storedToken.Expires = DateTime.UtcNow;

			_context.RefreshTokens.Update(storedToken);

			await _context.SaveChangesAsync(cancellationToken);

			return (newJwtToken, newRefreshToken);
		}
	}
}
