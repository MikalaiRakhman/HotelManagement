using HotelManagement.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Services
{
	public static class BookingsService
	{
		public static async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId, IApplicationDbContext context)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;

			var room = await context.Rooms.FindAsync(roomId);
			Guard.AgainstNull(room, nameof(room));

			return differenceInDays * room.PricePerNight;
		}

		public static async Task<bool> CheckRoomAvailibilityAsync(Guid roomId, DateOnly startDate, DateOnly endDate, IApplicationDbContext context)
		{
			var bookingsWhereRoomId = await context.Bookings
				.Where(b => b.RoomId == roomId)
				.ToListAsync();

			return !bookingsWhereRoomId.Any(booking => DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDate));
		}

		public static bool DoDateRangesOverlap(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2)
		{
			return startDate1 <= endDate2 && startDate2 <= endDate1;
		}
	}
}
