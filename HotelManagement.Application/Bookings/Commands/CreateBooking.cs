using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands
{
	public class CreateBooking : IRequest<Guid>
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalPrice { get; set; }
	}

	public class CreateBookingHandler : IRequestHandler<CreateBooking, Guid>
	{
		private readonly IApplicationDbContext _context;

		public CreateBookingHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Handle(CreateBooking request, CancellationToken cancellationToken)
		{
			var entity = new Booking
			{
				UserId = request.UserId,
				RoomId = request.RoomId,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				TotalPrice = request.TotalPrice,
			};

			_context.Bookings.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.UserId;
		}
	}
}
