using HotelManagement.Application.Bookings.Services;
using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands.Create
{
	public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
	{
		private readonly IApplicationDbContext _context;
		private readonly BookingsService _bookingsService;

		public CreateBookingCommandHandler(IApplicationDbContext context, BookingsService bookingsService)
		{
			_context = context;
			_bookingsService = bookingsService;
		}

		public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
		{
			Guard.AgainstNullOrEmpty(request.UserId, nameof(request.UserId));
			Guard.AgainstNullOrEmpty(request.RoomId, nameof(request.RoomId));

			if (request.StartDate < DateOnly.FromDateTime(DateTime.UtcNow))
			{
				throw new Exception("The booking day cannot be before the current day.");
			}

			if (await _bookingsService.CheckRoomAvailibilityAsync(request.RoomId, request.StartDate, request.EndDate))
			{
				var user = await _context.Users.FindAsync(request.UserId);
				Guard.AgainstNull(user, nameof(user));

				var room = await _context.Rooms.FindAsync(request.RoomId);
				Guard.AgainstNull(room, nameof(room));

				var entity = new Booking
				{
					UserId = request.UserId,
					RoomId = request.RoomId,
					StartDate = request.StartDate,
					EndDate = request.EndDate,
					TotalPrice = await _bookingsService.CalculateTheCostOfBooking(request.StartDate, request.EndDate, request.RoomId),
					User = user,
					Room = room
				};

				await _context.Bookings.AddAsync(entity);

				await _context.SaveChangesAsync(cancellationToken);

				return entity.UserId;
			}
			else
			{
				throw new Exception($"The room with id == {request.RoomId} is not availible on this dates.");
			}
		}
	}
}
