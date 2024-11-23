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
			if(await IsRoomAvailable(request.RoomId, request.StartDate, request.EndDate))
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

				return entity.UserId;
			}

			throw new Exception($"Room whith RoomId = {request.RoomId} is not available on this days.");
		}

		private async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;
			var pricePerNight = (await _context.Rooms.FindAsync(roomId)).PricePerNight;

			return differenceInDays * pricePerNight;
		}		

		private bool DoDateRangesOverlap(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2) 
		{
			return startDate1 <= endDate2 && startDate2 <= endDate1; 
		}

		private async Task<bool> IsRoomAvailable(Guid roomId, DateOnly startDate, DateOnly endDate) 
		{
			var room = await _context.Rooms.FindAsync(roomId);

			if (room == null) 
			{
				throw new Exception($"Room with ID = {roomId} is not exist.");
			}

			if (room.Bookings != null) 
			{
				foreach (var booking in room.Bookings)
				{
					if(DoDateRangesOverlap(startDate, endDate, booking.StartDate, booking.EndDate))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
