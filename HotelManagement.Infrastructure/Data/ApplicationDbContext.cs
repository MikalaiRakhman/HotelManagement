using Microsoft.EntityFrameworkCore;
using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;

namespace HotelManagement.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<User> Users {  get; set; }
		public DbSet<Room> Rooms {  get; set; }
		public DbSet<Booking> Bookings {  get; set; }

		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			base.OnModelCreating (modelBuilder);
		}
	}
}
