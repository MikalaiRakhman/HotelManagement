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
			var userFromRequest = await _context.Users.FindAsync(request.UserId);
			var roomFromRequest = await _context.Rooms.FindAsync(request.RoomId);

			if (IsCurrentDateInRange(request.StartDate, request.EndDate))
			{
				roomFromRequest.IsAvailable = false;
			}

			var entity = new Booking
			{
				UserId = request.UserId,
				RoomId = roomFromRequest.Id,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				TotalPrice = request.TotalPrice,
				User = userFromRequest,
				Room = roomFromRequest
			};

			await _context.Bookings.AddAsync(entity);

			roomFromRequest.Bookings.Add(entity);
			userFromRequest.Bookings.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.UserId;
		}

		private bool IsCurrentDateInRange(DateTime startDate, DateTime endDate) 
		{
			DateTime now = DateTime.Now; 
			return now >= startDate && now <= endDate; 
		}
	}
}
