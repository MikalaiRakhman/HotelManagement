using MediatR;

namespace HotelManagement.Application.Bookings.Commands.Create
{
	public class CreateBookingCommand : IRequest<Guid>
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
	}
}
