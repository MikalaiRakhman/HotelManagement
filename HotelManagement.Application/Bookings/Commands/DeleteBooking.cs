using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands
{
	public record DeleteBooking(Guid Id) : IRequest;

	public class DeleteBookingHandler : IRequestHandler<DeleteBooking>
	{
		private readonly IApplicationDbContext _context;

		public DeleteBookingHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteBooking request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings.FindAsync([request.Id], cancellationToken);
			Guard.AgainstNull(entity, nameof(entity));

			_context.Bookings.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
