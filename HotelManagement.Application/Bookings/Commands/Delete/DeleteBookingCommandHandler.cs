using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands.Delete
{
	public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
	{
		private readonly IApplicationDbContext _context;

		public DeleteBookingCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings.FindAsync([request.Id], cancellationToken);
			Guard.AgainstNull(entity, nameof(entity));

			_context.Bookings.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
