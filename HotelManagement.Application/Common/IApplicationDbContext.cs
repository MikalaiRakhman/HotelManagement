using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Common
{
    public interface IApplicationDbContext
    {
		DbSet<User> Users { get; }
		DbSet<Room> Rooms { get; }
		DbSet<Booking> Bookings { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
