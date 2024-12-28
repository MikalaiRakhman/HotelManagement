using HotelManagement.Application.Bookings.Queries.DTOs;
using MediatR;

namespace HotelManagement.Application.Bookings.Queries.GetBookingDetails
{
	public record GetBookingDetailsQuery : IRequest<BookingDetailsDTO>
	{
		public Guid Id { get; set; }

		public GetBookingDetailsQuery(Guid id)
		{
			Id = id;
		}
	}	
}