using HotelManagement.Application.Bookings.Services;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Commands.Update
{
	public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand>
	{
		private readonly IApplicationDbContext _context;
		private readonly BookingsService _bookingsService;

		public UpdateBookingCommandHandler(IApplicationDbContext context, BookingsService bookingsService)
		{
			_context = context;
			_bookingsService = bookingsService;
		}

		public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
		{
			if (await CheckRoomAvailibilityAsync(request.RoomId, request.StartDate, request.EndDate))
			{
				var entity = await _context.Bookings.FindAsync([request.Id], cancellationToken);
				Guard.AgainstNull(entity, nameof(entity));

				entity.UserId = request.UserId;
				entity.RoomId = request.RoomId;
				entity.StartDate = request.StartDate;
				entity.EndDate = request.EndDate;
				entity.TotalPrice = await CalculateTheCostOfBooking(request.StartDate, request.EndDate, request.RoomId);

				entity.User = await _context.Users.FindAsync(request.UserId);
				Guard.AgainstNull(entity.User, nameof(entity.User));

				entity.Room = await _context.Rooms.FindAsync(request.RoomId);
				Guard.AgainstNull(entity.Room, nameof(entity.Room));

				await _context.SaveChangesAsync(cancellationToken);
			}
			else
			{
				throw new Exception($"The room with id == {request.RoomId} is not availible on this dates.");
			}
		}

		private async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;

			var room = await _context.Rooms.FindAsync(roomId);
			Guard.AgainstNull(room, nameof(room));

			return differenceInDays * room.PricePerNight;
		}

		private async Task<bool> CheckRoomAvailibilityAsync(Guid roomId, DateOnly startDate, DateOnly endDate)
		{
			var bookingsWhereRoomId = await _context.Bookings
				.Where(b => b.RoomId == roomId)
				.ToListAsync();

			return !bookingsWhereRoomId.Any(booking => _bookingsService.DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDate));
		}
	}
}