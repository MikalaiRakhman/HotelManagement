using HotelManagement.Application.Bookings.Queries.DTOs;
using MediatR;

namespace HotelManagement.Application.Bookings.Queries.GetBookingsByRoomId
{
	public record GetBookingsByRoomIdQuery : IRequest<List<BookingDTO>>
	{
		public Guid RoomId { get; set; }

		public GetBookingsByRoomIdQuery(Guid roomId)
		{
			RoomId = roomId;
		}
	}	
}