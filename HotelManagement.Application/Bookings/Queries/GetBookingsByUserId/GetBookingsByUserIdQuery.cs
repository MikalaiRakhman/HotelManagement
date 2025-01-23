using HotelManagement.Application.Bookings.Queries.DTOs;
using MediatR;

namespace HotelManagement.Application.Bookings.Queries.GetBookingsByUserId
{
	public record GetBookingsByUserIdQuery : IRequest<List<BookingDTO>>
	{
		public Guid UserId { get; set; }

		public GetBookingsByUserIdQuery(Guid userId)
		{
			UserId = userId;
		}
	}	
}