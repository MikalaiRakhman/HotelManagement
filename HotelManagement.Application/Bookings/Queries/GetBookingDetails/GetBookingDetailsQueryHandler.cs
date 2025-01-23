using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries.GetBookingDetails
{
	public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, BookingDetailsDTO>
	{
		private readonly IApplicationDbContext _context;

		public GetBookingDetailsQueryHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BookingDetailsDTO> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings
				.Where(b => b.Id == request.Id)
				.Select(b => new BookingDetailsDTO
				{
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
					BookerFullName = $"Booker is {b.User.FirstName} {b.User.LastName}",
					RoomDetails = $"Room number is {b.Room.RoomNumber}. Room type is {b.Room.RoomType}. Price for one night is {b.Room.PricePerNight}."
				}).FirstOrDefaultAsync(cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			return entity;
		}
	}
}