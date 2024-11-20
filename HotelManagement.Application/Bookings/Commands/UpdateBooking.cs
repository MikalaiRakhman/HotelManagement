using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands
{
	public record UpdateBooking : IRequest
	{
		public Guid Id { get; init; }
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }

		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int TotalPrice { get; set; }
	}

	public class UpdateBookingHandler : IRequestHandler<UpdateBooking>
	{
		private readonly IApplicationDbContext _context;

		public UpdateBookingHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateBooking request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings.FindAsync([request.Id], cancellationToken);

			if (entity == null)
			{
				throw new Exception($"Entity with Id = {request.Id} was not found!");
			}

			entity.UserId = request.UserId;
			entity.RoomId = request.RoomId;
			entity.StartDate = request.StartDate;
			entity.EndDate = request.EndDate;
			entity.TotalPrice = request.TotalPrice;
			entity.User = await _context.Users.FindAsync(request.UserId);
			entity.Room = await _context.Rooms.FindAsync(request.RoomId);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
