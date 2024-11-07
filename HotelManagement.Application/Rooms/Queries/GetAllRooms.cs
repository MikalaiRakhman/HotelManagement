using HotelManagement.Application.Common;
using HotelManagement.Application.Users.Queries;
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
			var roomsDTO = new List<RoomDTO>();
			var allRooms = await _context.Rooms.ToListAsync(cancellationToken);

			foreach (var room in allRooms) 
			{
				var bookingsIdFromRoom = new List<Guid>();

				if (room.Bookings != null)
				{
					foreach (var booking in room.Bookings) 
					{ 
						bookingsIdFromRoom.Add(booking.Id);
					}
				}

				roomsDTO.Add(new RoomDTO
				{
					Id = room.Id,
					RoomNumber = room.RoomNumber,
					RoomType = room.RoomType,
					PricePerNight = room.PricePerNight,
					IsAvailable = room.IsAvailable,
					BookingsId = bookingsIdFromRoom
				});				
			}

			return roomsDTO;
		}
	}
}
