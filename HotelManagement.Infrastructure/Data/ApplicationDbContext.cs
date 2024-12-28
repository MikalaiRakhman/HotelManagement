using Microsoft.EntityFrameworkCore;
using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HotelManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users {  get; set; }
		public DbSet<Room> Rooms {  get; set; }
		public DbSet<Booking> Bookings {  get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); 
			
			modelBuilder.Entity<Room>()
				.HasMany(r => r.Bookings)
				.WithOne(b => b.Room)
				.HasForeignKey(b => b.RoomId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<User>()
				.HasMany(u => u.Bookings)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<RefreshToken>()
				.HasIndex(rt => rt.Token)
				.IsUnique();

			modelBuilder.Entity<RefreshToken>()
				.HasOne(rt => rt.ApplicationUser)
				.WithMany()
				.HasForeignKey(rt => rt.ApplicationUserId);
		}
	}
}