using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries.GetBookingsByRoomId
{
	public class GetBookingsByRoomIdQueryHandler : IRequestHandler<GetBookingsByRoomIdQuery, List<BookingDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetBookingsByRoomIdQueryHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<BookingDTO>> Handle(GetBookingsByRoomIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings
				.Where(b => b.RoomId == request.RoomId)
				.Select(b => new BookingDTO
				{
					Id = b.Id,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
				}).ToListAsync(cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			return entity;
		}
	}
}