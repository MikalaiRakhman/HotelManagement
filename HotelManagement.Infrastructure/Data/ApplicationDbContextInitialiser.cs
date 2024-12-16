using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HotelManagement.Infrastructure.Data
{
	public class ApplicationDbContextInitialiser
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly ILogger _logger;

		public ApplicationDbContextInitialiser(
			ApplicationDbContext context, 
			RoleManager<IdentityRole<Guid>> roleManager,
			UserManager<ApplicationUser> userManager,
			ILogger logger)
		{
			_context = context;
			_roleManager = roleManager;
			_userManager = userManager;
			_logger = logger;
		}

		public async Task InitialiseAsync()
		{
			_logger.Information("Initializing database...");
			await _context.Database.MigrateAsync();
			_logger.Information("Database initialized successfully.");
		}

		public async Task SeedAsync()
		{
			_logger.Information("Seeding roles...");
			foreach (var role in Enum.GetNames(typeof(ApplicationRole)))
			{
				if (!await _roleManager.RoleExistsAsync(role)) 
				{
					_logger.Information($"Creating role: {role}");
					await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
				}
			}

			var adminEmail = "admin@test.com";

			_logger.Information($"Checking if admin user exists: {adminEmail}");
			var adminUser = await _userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				_logger.Information($"Creating admin user: {adminEmail}");
				adminUser = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail
				};

				var result = await _userManager.CreateAsync(adminUser, "AdminPassword!1");

				if (result.Succeeded)
				{
					_logger.Information("Admin user created successfully.");
					await _userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin.ToString());
					_logger.Information("Admin user assigned to Admin role.");
				}
			}

			_logger.Information($"Checking if user exists in Users table: {adminEmail}");
			var theSameUserInDatabase = await _context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

			if (theSameUserInDatabase == null)
			{
				_logger.Information("Converting ApplicationUser to User entity and adding to database.");
				var userConvertFromAppUserToUser = new User
				{
					Email = adminUser.Email
				};

				await _context.AddAsync(userConvertFromAppUserToUser);
				await _context.SaveChangesAsync();
				_logger.Information("User entity added to database successfully.");
			}
		}
	}
}
