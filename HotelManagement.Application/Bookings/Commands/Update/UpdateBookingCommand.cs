using MediatR;

namespace HotelManagement.Application.Bookings.Commands.Update
{
	public record UpdateBookingCommand : IRequest
	{
		public Guid Id { get; init; }
		public Guid UserId { get; init; }
		public Guid RoomId { get; init; }
		public DateOnly StartDate { get; init; }
		public DateOnly EndDate { get; init; }
	}	
}