using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Identity;
using HotelManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly TokenProvider _tokenProvider;
		private readonly IApplicationDbContext _context;

		public AuthController(UserManager<ApplicationUser> userManager, TokenProvider tokenProvider, IApplicationDbContext context)
		{
			_tokenProvider = tokenProvider;
			_userManager = userManager;
			_context = context;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var appUser = new ApplicationUser
			{
				Email = model.Email,
				UserName = model.Email
			};

			var result = await _userManager.CreateAsync(appUser, model.Password);

			if (!result.Succeeded) 
			{
				return BadRequest(result.Errors);
			}

			await _userManager.AddToRoleAsync(appUser, ApplicationRole.User.ToString());

			await _context.Users.AddAsync(ConvertToDomainUser(appUser));

			await _context.SaveChangesAsync();

			return Ok(new { Message = "User registered successfully!" });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
		{
			var applicationUser = await _userManager.FindByEmailAsync(model.Email);

			Guard.AgainstUnauthorized(applicationUser);

			var  isValidPassword = await _userManager.CheckPasswordAsync(applicationUser, model.Password);

			Guard.AgainsInvalidPassword(isValidPassword);
			
			var roles = await _userManager.GetRolesAsync(applicationUser);
			var token = _tokenProvider.GenerateJwtToken(applicationUser, roles);
			var refreshToken = _tokenProvider.GenerateRefreshToken(applicationUser.Id, cancellationToken);

			return Ok(new {Token = token, RefreshToken = refreshToken.Result});
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model, CancellationToken cancellationToken)
		{
			try
			{
				var (newJwtToken, newRefreshToken) = await _tokenProvider.RefreshTokens(model.RefreshToken, cancellationToken);

				return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { Error = ex.Message });
			}
		}

		private User ConvertToDomainUser(ApplicationUser appUser)
		{
			return new User
			{				
				Email = appUser.Email,			
			};
		}
	}
}
