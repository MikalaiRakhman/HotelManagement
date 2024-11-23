using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Rooms.Queries
{
	public record class GetAllRooms : IRequest<List<RoomDTO>>
	{
	}

	public class GetAllRoomsHandler : IRequestHandler<GetAllRooms, List<RoomDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllRoomsHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<RoomDTO>> Handle(GetAllRooms request, CancellationToken cancellationToken)
		{
			await UpdateRoomsAvailabilityForNow(cancellationToken);

			return await _context.Rooms.Select(r => new RoomDTO
			{
				Id = r.Id,
				RoomNumber = r.RoomNumber,
				RoomType = r.RoomType,
				PricePerNight = r.PricePerNight,
				IsAvailable = r.IsAvailable,
			}).ToListAsync(cancellationToken);			
		}

		private bool IsThisRoomAvailibleNow(Room room)
		{
			if (room.Bookings.Any())
			{
				foreach (var booking in room.Bookings)
				{
					if (IsCurrentDateInRange(booking.StartDate, booking.EndDate))
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool IsCurrentDateInRange(DateOnly startDate, DateOnly endDate)
		{
			DateOnly now = DateOnly.FromDateTime(DateTime.Now);

			return now >= startDate && now <= endDate;
		}

		private async Task UpdateRoomsAvailabilityForNow(CancellationToken cancellationToken)
		{
			var rooms = await _context.Rooms.ToListAsync(cancellationToken);

			foreach (var room in rooms)
			{
				room.IsAvailable = IsThisRoomAvailibleNow(room);
			}

			await _context.SaveChangesAsync();
		}
	}
}
