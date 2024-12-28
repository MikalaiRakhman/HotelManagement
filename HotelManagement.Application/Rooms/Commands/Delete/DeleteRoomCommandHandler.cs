using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Delete
{
	public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
	{
		private readonly IApplicationDbContext _context;

		public DeleteRoomCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Rooms.FindAsync([request.Id], cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			_context.Rooms.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}