using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands
{
	public record DeleteRoom (Guid Id) : IRequest;	

	public class DeleteRoomHandler : IRequestHandler<DeleteRoom>
	{
		private readonly IApplicationDbContext _context;

		public DeleteRoomHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteRoom request, CancellationToken cancellationToken)
		{
			var entity = await _context.Rooms.FindAsync([request.Id], cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			_context.Rooms.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
