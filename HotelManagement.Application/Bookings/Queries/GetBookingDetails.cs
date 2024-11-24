using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries
{
	public record GetBookingDetails : IRequest<BookingDetailsDTO>
	{
		public Guid Id { get; set; }

		public GetBookingDetails(Guid id)
		{
			Id = id;
		}
	}

	public class GetBookingDetailsHandler : IRequestHandler<GetBookingDetails, BookingDetailsDTO> 
	{
		private readonly IApplicationDbContext _context;

		public GetBookingDetailsHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BookingDetailsDTO> Handle(GetBookingDetails request, CancellationToken cancellationToken)
		{
			var bookingDetails =  await _context.Bookings
				.Where(b => b.Id == request.Id)
				.Select(b => new BookingDetailsDTO
				{
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
					BookerFullName = $"Booker is {b.User.FirstName} {b.User.LastName}",
					RoomDetails = $"Room number is {b.Room.RoomNumber}. Room type is {b.Room.RoomType}. Price for one night is {b.Room.PricePerNight}."
				}).FirstOrDefaultAsync(cancellationToken);

			if (bookingDetails == null) 
			{
				throw new Exception($"Booking with {request.Id} was not found");
			}

			return bookingDetails;
		}
	}
}
