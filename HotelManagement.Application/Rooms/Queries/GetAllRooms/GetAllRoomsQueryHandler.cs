using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Rooms.Queries.GetAllRooms
{
	public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllRoomsQueryHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<RoomDTO>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
		{
			await UpdateRoomsAvailabilityForNowAsync(cancellationToken);

			return await _context.Rooms.Select(r => new RoomDTO
			{
				Id = r.Id,
				RoomNumber = r.RoomNumber,
				RoomType = r.RoomType,
				PricePerNight = r.PricePerNight,
				IsAvailable = r.IsAvailable,
			}).ToListAsync(cancellationToken);
		}

		private bool IsCurrentDateInRange(DateOnly startDate, DateOnly endDate)
		{
			DateOnly now = DateOnly.FromDateTime(DateTime.Now);

			return now >= startDate && now <= endDate;
		}

		private async Task UpdateRoomsAvailabilityForNowAsync(CancellationToken cancellationToken)
		{
			var bookings = await _context.Bookings.ToListAsync(cancellationToken);

			foreach (var booking in bookings)
			{
				if (IsCurrentDateInRange(booking.StartDate, booking.EndDate))
				{
					//Комната точно будет существовать?
					var room = await _context.Rooms.FindAsync(booking.RoomId, cancellationToken);

					room.IsAvailable = false;
				}
				else
				{
					var room = await _context.Rooms.FindAsync(booking.RoomId, cancellationToken);

					room.IsAvailable = true;
				}
			}

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}