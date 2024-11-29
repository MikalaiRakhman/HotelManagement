using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Commands
{
	public class CreateBooking : IRequest<Guid>
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
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
			Guard.AgainstNullOrEmpty(request.UserId, nameof(request.UserId));
			Guard.AgainstNullOrEmpty(request.RoomId, nameof(request.RoomId));

			if (await CheckRoomAvailibilityAsync(request.RoomId ,request.StartDate, request.EndDate))
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
					TotalPrice = await CalculateTheCostOfBooking(request.StartDate, request.EndDate, request.RoomId),
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

			return !bookingsWhereRoomId.Any(booking => DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDate));
		}

		public bool DoDateRangesOverlap(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2)
		{ 
			return startDate1 <= endDate2 && startDate2 <= endDate1; 
		}
	}
}
