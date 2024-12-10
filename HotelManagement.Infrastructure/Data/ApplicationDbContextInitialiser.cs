using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Infrastructure.Data
{
	public class ApplicationDbContextInitialiser
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;

		public ApplicationDbContextInitialiser(
			ApplicationDbContext context, 
			RoleManager<IdentityRole<Guid>> roleManager,
			UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task InitialiseAsync()
		{			
			await _context.Database.MigrateAsync();
		}

		public async Task SeedAsync()
		{
			foreach (var role in Enum.GetNames(typeof(ApplicationRole)))
			{
				if (!await _roleManager.RoleExistsAsync(role)) 
				{
					await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
				}
			}

			var adminEmail = "admin@test.com";
			var adminUser = await _userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				adminUser = new ApplicationUser
				{
					UserName = adminEmail,
					FirstName = "FirstNameAdmin",
					LastName = "LastNameAdmin",
					Email = adminEmail
				};

				var result = await _userManager.CreateAsync(adminUser, "AdminPassword!1");

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin.ToString());
				}
			}

			var userConvertFromAppUserToUser = new User
			{
				FirstName = adminUser.FirstName,
				LastName = adminUser.LastName,
				Email = adminUser.Email
			};

			await _context.AddAsync(userConvertFromAppUserToUser);
			
			await _context.SaveChangesAsync();
		}
	}
}
