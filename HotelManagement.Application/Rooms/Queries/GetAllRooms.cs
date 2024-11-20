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
			return await _context.Rooms.Select(r => new RoomDTO
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
