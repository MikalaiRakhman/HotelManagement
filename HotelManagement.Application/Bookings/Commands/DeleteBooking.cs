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

			if (entity == null) 
			{
				throw new Exception($"Entity with Id = {request.Id} was not found.");
			}

			_context.Bookings.Remove(entity);
			await DeleteBookingFromUserBookingsAsync(entity.UserId, entity.Id);
			await DeleteBookingFromRoomBookingsAsync(entity.RoomId, entity.Id);

			await _context.SaveChangesAsync(cancellationToken);
		}

		private async Task DeleteBookingFromUserBookingsAsync(Guid userId, Guid bookingId)
		{
			var user = await _context.Users.FindAsync(userId);

			var booking = user.Bookings.FirstOrDefault(x => x.Id == bookingId);

			user.Bookings.Remove(booking);
		}

		private async Task DeleteBookingFromRoomBookingsAsync(Guid roomId, Guid bookingId)
		{
			var room = await _context.Rooms.FindAsync(roomId);

			var booking = room.Bookings.FirstOrDefault(x => x.Id == bookingId);

			room.Bookings.Remove(booking);
		}
	}
}
