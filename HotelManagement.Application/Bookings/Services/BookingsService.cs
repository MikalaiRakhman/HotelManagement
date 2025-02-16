using HotelManagement.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Services
{
	public class BookingsService
	{
		private readonly IApplicationDbContext _context;

		public BookingsService(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;

			var room = await _context.Rooms.FindAsync(roomId);
			Guard.AgainstNull(room, nameof(room));

			return differenceInDays * room.PricePerNight;
		}

		public async Task<bool> CheckRoomAvailibilityAsync(Guid roomId, DateOnly startDate, DateOnly endDate)
		{
			var bookingsWhereRoomId = await _context.Bookings
				.Where(b => b.RoomId == roomId)
				.ToListAsync();

			return !bookingsWhereRoomId.Any(booking => DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDate));
		}

		public bool DoDateRangesOverlap(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2)
		{
			return startDate1 <= endDate2 && startDate2 <= endDate1;
		}
	}
}
