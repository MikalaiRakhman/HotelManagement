using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Update
{
	public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
	{
		private readonly IApplicationDbContext _context;

		public UpdateRoomCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Rooms.FindAsync(request.Id, cancellationToken);
			Guard.AgainstNull(entity, nameof(entity));

			entity.RoomNumber = request.RoomNumber;
			entity.RoomType = request.RoomType;
			entity.PricePerNight = request.PricePerNight;
			entity.IsAvailable = true;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}