using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Rooms.Queries
{
	public record class GetAllRooms : IRequest<List<Room>>
	{
	}

	public class GetAllRoomsHandler : IRequestHandler<GetAllRooms, List<Room>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllRoomsHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Room>> Handle(GetAllRooms request, CancellationToken cancellationToken)
		{
			return await _context.Rooms
				.Select(r => new Room
				{
					Id = r.Id,
					RoomNumber = r.RoomNumber,
					RoomType = r.RoomType,
					PricePerNight = r.PricePerNight,
					IsAvailable = r.IsAvailable,
				}).ToListAsync(cancellationToken);
		}
	}
}
