using HotelManagement.Application.Common;
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
