using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;
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

		public TokenProvider(IConfiguration configuration, ApplicationDbContext context)
		{
			_configuration = configuration;
			_context = context;
		}

		public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.FirstName.ToString()),
				new Claim(ClaimTypes.Surname, user.LastName.ToString())
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
	}
}
