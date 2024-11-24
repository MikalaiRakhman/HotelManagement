using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

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
			if(await CheckRoomAvailibilityAsync(request.RoomId, request.StartDate, request.EndDate))
			{
				var entity = new Booking
				{
					UserId = request.UserId,
					RoomId = request.RoomId,
					StartDate = request.StartDate,
					EndDate = request.EndDate,
					TotalPrice = await CalculateTheCostOfBooking(request.StartDate, request.EndDate, request.RoomId),
					User = await _context.Users.FindAsync(request.UserId),
					Room = await _context.Rooms.FindAsync(request.RoomId)
				};

				await _context.Bookings.AddAsync(entity);

				await _context.SaveChangesAsync(cancellationToken);

				await AddBookingToRoomAsync(entity.RoomId, entity);
				await AddBookingToUserAsync(entity.UserId, entity);

				await _context.SaveChangesAsync(cancellationToken);

				return entity.UserId;
			}
			else
			{
				throw new Exception($"The room with id == {request.RoomId} is not availible on this dates.");
			}			
		}

		private async Task AddBookingToRoomAsync(Guid roomId, Booking booking)
		{
			var room = await _context.Rooms.FindAsync(roomId);

			room.Bookings.Add(booking);
		}

		private async Task AddBookingToUserAsync(Guid userId, Booking booking)
		{
			var user = await _context.Users.FindAsync(userId);

			user.Bookings.Add(booking);
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
				if(DoDateRangesOverlap(booking.StartDate, booking.EndDate, startDate, endDay))
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
