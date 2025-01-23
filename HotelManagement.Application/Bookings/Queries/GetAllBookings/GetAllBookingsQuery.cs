using HotelManagement.Application.Bookings.Queries.DTOs;
using MediatR;

namespace HotelManagement.Application.Bookings.Queries.GetAllBookings
{
	public record GetAllBookingsQuery : IRequest<List<BookingDTO>>
	{
	}	
}