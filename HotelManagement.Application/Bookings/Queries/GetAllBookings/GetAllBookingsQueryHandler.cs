using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries.GetAllBookings
{
	public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, List<BookingDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllBookingsQueryHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<BookingDTO>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
		{
			return await _context.Bookings
				.Select(b => new BookingDTO
				{
					Id = b.Id,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
					BookerEmail = b.User.Email,
					RoomNumber = b.Room.RoomNumber
				}).ToListAsync(cancellationToken);
		}
	}
}