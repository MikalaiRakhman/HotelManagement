using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using HotelManagement.Domain.Entities.Enums;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands
{
	public class CreateRoom : IRequest<Guid>
	{
		public int RoomNumber { get; set; }
		public RoomType RoomType { get; set; }
		public int PricePerNight { get; set; }
	}

	public class CreateRoomHandler : IRequestHandler<CreateRoom, Guid>
	{
		private readonly IApplicationDbContext _context;

		public CreateRoomHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Handle(CreateRoom request, CancellationToken cancellationToken)
		{
			var entity = new Room
			{
				RoomNumber = request.RoomNumber,
				RoomType = request.RoomType,
				PricePerNight = request.PricePerNight,
				IsAvailable = true,
				CreatedAt = DateTime.Now,
				LastModifiedAt = DateTime.Now
			};

			_context.Rooms.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
