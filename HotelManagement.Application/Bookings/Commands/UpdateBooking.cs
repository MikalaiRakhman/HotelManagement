using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

namespace HotelManagement.Application.Bookings.Commands
{
	public record UpdateBooking : IRequest
	{
		public Guid Id { get; init; }
		public Guid UserId { get; init; }
		public Guid RoomId { get; init; }

		public DateOnly StartDate { get; init; }
		public DateOnly EndDate { get; init; }		
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
			if(await CheckRoomAvailibilityAsync(request.RoomId, request.StartDate, request.EndDate))
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
				entity.TotalPrice = await CalculateTheCostOfBooking(request.StartDate, request.EndDate, request.RoomId);
				entity.User = await _context.Users.FindAsync(request.UserId);
				entity.Room = await _context.Rooms.FindAsync(request.RoomId);

				await _context.SaveChangesAsync(cancellationToken);

				var newEntity = await _context.Bookings.FindAsync(request.Id);

				await FindAndChangeBookingOrAddNewInUserAsync(request.UserId, newEntity);
				await FindAndChangeBookingOrAddNewInRoomAsync(request.RoomId, newEntity);

				await _context.SaveChangesAsync(cancellationToken);
			}
			else
			{
				throw new Exception($"The room with id == {request.RoomId} is not availible on this dates.");
			}						
		}
		
		private async Task FindAndChangeBookingOrAddNewInUserAsync(Guid userId, Booking booking)
		{
			var user = await _context.Users.FindAsync(userId);

			if(user.Bookings == null)
			{
				await AddBookingToUserAsync(userId, booking);
			}
			else
			{
				user.Bookings.Add(booking);
			}
		}

		private async Task FindAndChangeBookingOrAddNewInRoomAsync(Guid roomId, Booking booking)
		{
			var room = await _context.Rooms.FindAsync(roomId);

			if (room.Bookings == null)
			{
				await AddBookingToUserAsync(roomId, booking);
			}
			else
			{
				room.Bookings.Add(booking);
			}
		}

		private async Task AddBookingToUserAsync(Guid userId, Booking booking)
		{
			var user = await _context.Users.FindAsync(userId);

			user.Bookings.Add(booking);
		}

		private async Task AddBookingToRoomAsync(Guid roomId, Booking booking)
		{
			var room = await _context.Rooms.FindAsync(roomId);

			room.Bookings.Add(booking);
		}

		private async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;
			var pricePerNight = (await _context.Rooms.FindAsync(roomId)).PricePerNight;

			return differenceInDays * pricePerNight;
		}

		private async Task<bool> CheckRoomAvailibilityAsync(Guid roomId, DateOnly startDate, DateOnly endDay)
		{
			var room = await _context.Rooms.FindAsync(roomId);

			if (room.Bookings == null || !room.Bookings.Any())
			{
				return true;
			}

			foreach (var booking in room.Bookings)
			{
				if (DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDay))
				{
					return false;
				}
			}

			return true;
		}

		public bool DoDateRangesOverlap(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2)
		{
			return startDate1 <= endDate2 && startDate2 <= endDate1;
		}
	}
}
