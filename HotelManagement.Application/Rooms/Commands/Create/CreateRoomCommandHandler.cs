using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Create
{
	public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
	{
		private readonly IApplicationDbContext _context;

		public CreateRoomCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
		{
			var entity = new Room
			{
				RoomNumber = request.RoomNumber,
				RoomType = request.RoomType,
				PricePerNight = request.PricePerNight,
				IsAvailable = true,
			};

			_context.Rooms.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}