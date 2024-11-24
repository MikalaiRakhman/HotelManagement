using HotelManagement.Application.Common;
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
		}

		private async Task<int> CalculateTheCostOfBooking(DateOnly startDay, DateOnly endDay, Guid roomId)
		{
			var differenceInDays = endDay.DayNumber - startDay.DayNumber;
			var pricePerNight = (await _context.Rooms.FindAsync(roomId)).PricePerNight;

			return differenceInDays * pricePerNight;
		}
	}
}
