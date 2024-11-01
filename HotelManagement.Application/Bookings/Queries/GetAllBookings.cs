using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries
{
	public record GetAllBookings : IRequest<List<Booking>>
	{
	}

	public class GetAllBookingsHandler : IRequestHandler<GetAllBookings, List<Booking>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllBookingsHandler(IApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<List<Booking>> Handle(GetAllBookings request, CancellationToken cancellationToken)
		{
			return await _context.Bookings
				.Select(b => new Booking
				{
					Id = b.Id,
					UserId = b.UserId,
					RoomId = b.RoomId,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
				}).ToListAsync(cancellationToken);				
		}
	}
}
