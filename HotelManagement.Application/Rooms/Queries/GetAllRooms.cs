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
			var count = 0;

			if (room.Bookings != null)
			{
				foreach (var booking in room.Bookings)
				{
					if (IsCurrentDateInRange(booking.StartDate, booking.EndDate))
					{
						count++;
					}
				}

				if (count > 0)
				{
					return false;
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
				if (room.Bookings != null) 
				{
					if(room.Bookings.Count > 0)
					{
						foreach (var booking in room.Bookings)
						{
							if(IsCurrentDateInRange(booking.StartDate, booking.EndDate))
							{
								room.IsAvailable = false;
							}
							else
							{
								room.IsAvailable = true;
							}
						}
					}
				}
			}

			await _context.SaveChangesAsync();
		}
	}
}
