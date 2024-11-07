using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries
{
	public record GetAllBookings : IRequest<List<BookingDTO>>
	{
	}

	public class GetAllBookingsHandler : IRequestHandler<GetAllBookings, List<BookingDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllBookingsHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<BookingDTO>> Handle(GetAllBookings request, CancellationToken cancellationToken)
		{
			return await _context.Bookings
				.Select(b => new BookingDTO
				{
					Id = b.Id,
					UserId = b.UserId,
					RoomId = b.RoomId,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice
					
				}).ToListAsync(cancellationToken);				
		}
	}
}
