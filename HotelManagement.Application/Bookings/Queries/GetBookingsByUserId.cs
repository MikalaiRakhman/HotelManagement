using HotelManagement.Application.Bookings.Queries.DTOs;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Bookings.Queries
{
	public record GetBookingsByUserId : IRequest<List<BookingDTO>>
	{
		public Guid UserId { get; set; }

		public GetBookingsByUserId(Guid userId)
		{
			UserId = userId;
		}
	}

	public class GetBookingsByUserIdHandler : IRequestHandler<GetBookingsByUserId, List<BookingDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetBookingsByUserIdHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<BookingDTO>> Handle(GetBookingsByUserId request, CancellationToken cancellationToken)
		{
			var entity = await _context.Bookings
				.Where(b => b.UserId == request.UserId)
				.Select(b => new BookingDTO
				{
					Id = b.Id,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					TotalPrice = b.TotalPrice
				}).ToListAsync(cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			return entity;
		}
	}
}
