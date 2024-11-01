using HotelManagement.Domain.Entities;
using HotelManagement.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Infrastructure.Data
{
	public class ApplicationDbContextInitialiser
	{
		private readonly ApplicationDbContext _context;

		public ApplicationDbContextInitialiser(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task InitialiseAsync()
		{
			try 
			{
				await _context.Database.MigrateAsync();
			}
			catch (Exception ex) 
			{

			}
		}

		public async Task SeedAsync()
		{
			try
			{
				var adminId = Guid.NewGuid();
				var suitRoomId = Guid.NewGuid();

				if (!_context.Users.Any()) 
				{
					await _context.Users.AddRangeAsync
						(
							new User
							{
								Id = adminId,
								FirstName = "Mikalai",
								LastName = "Rakhman",
								Email = "rakhmanmikalai@gmail.com"
							},
							new User
							{
								FirstName = "Adam",
								LastName = "Mitskevich",
								Email = "adammitskevich@bel.com"
							},
							new User
							{
								FirstName = "Mike",
								LastName = "Tyson",
								Email = "champ@boxing.com"
							}
						);

					await _context.SaveChangesAsync();
				}
				if (!_context.Rooms.Any())
				{
					await _context.Rooms.AddRangeAsync
						(
							new Room
							{
								Id = suitRoomId,
								RoomNumber = 1,
								RoomType = RoomType.Suite,
								PricePerNight = 1000,
								IsAvailable = true,
							},
							new Room
							{
								RoomNumber = 2,
								RoomType = RoomType.Double_Room,
								PricePerNight = 200,
								IsAvailable = true,
							},
							new Room
							{
								RoomNumber = 3,
								RoomType = RoomType.King_Room,
								PricePerNight = 500,
								IsAvailable = true,
							},
							new Room
							{
								RoomNumber = 4,
								RoomType = RoomType.Single_Room,
								PricePerNight = 100,
								IsAvailable = true,
							}
						);

					await _context.SaveChangesAsync();
				}
				if (!_context.Bookings.Any())
				{
					await _context.Bookings.AddAsync
						(
							new Booking
							{
								UserId = adminId,
								RoomId = suitRoomId,
								StartDate = DateTime.Now,
								EndDate = DateTime.Now.AddDays(3),
								TotalPrice = 3000
							}
						);

					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{

			}
		}
	}
}
