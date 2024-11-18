using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries
{
	public record GetBookingsByRoomId : IRequest<List<BookingDTO>>
	{
		public Guid RoomId { get; set; }

		public GetBookingsByRoomId(Guid roomId)
		{
			RoomId = roomId;
		}
	}	
	
	public class GetBookingsByRoomIdHandler : IRequestHandler<GetBookingsByRoomId, List<BookingDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetBookingsByRoomIdHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<BookingDTO>> Handle(GetBookingsByRoomId request, CancellationToken cancellationToken)
		{
			return await _context.Bookings
				.Where(b => b.RoomId == request.RoomId)
				.Select(b => new BookingDTO
				{
					Id = b.Id,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice,
				}).ToListAsync(cancellationToken);
		}
	}
}
