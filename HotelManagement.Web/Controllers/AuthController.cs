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
				FirstName = model.FirstName,
				LastName = model.LastName,
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
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			Guard.AgainstUnauthorized(user);

			var  isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

			Guard.AgainsInvalidPassword(isValidPassword);
			
			var roles = await _userManager.GetRolesAsync(user);
			var token = _tokenProvider.GenerateJwtToken(user, roles);

			return Ok(new {Token = token});
		}

		private User ConvertToDomainUser(ApplicationUser appUser)
		{
			return new User
			{				
				Email = appUser.Email,
				FirstName = appUser.FirstName,
				LastName = appUser.LastName,				
			};
		}

	}
}
