using MediatR;

namespace HotelManagement.Application.Bookings.Commands.Delete
{
	public record DeleteBookingCommand(Guid Id) : IRequest;
}
