using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using HotelManagement.Domain.Entities.Enums;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands
{
	public record UpdateRoom : IRequest
	{
		public Guid Id { get; init; }
		public int RoomNumber { get; init; }
		public RoomType RoomType { get; init; }
		public int PricePerNight { get; init; }
	}

	public class UpdateRoomHandler : IRequestHandler<UpdateRoom>
	{
		private readonly IApplicationDbContext _context;

		public UpdateRoomHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateRoom request, CancellationToken cancellationToken)
		{
			var entity = await _context.Rooms.FindAsync(request.Id, cancellationToken);

			if (entity == null)
			{
				throw new Exception($"Entity with Id = {request.Id} was not found!");
			}

			entity.RoomNumber = request.RoomNumber;
			entity.RoomType = request.RoomType;
			entity.PricePerNight = request.PricePerNight;
			entity.IsAvailable = IsThisRoomAvailibleNow(entity);
			entity.LastModifiedAt = DateTime.Now;

			await _context.SaveChangesAsync(cancellationToken);
		}

		private bool IsCurrentDateInRange(DateOnly startDate, DateOnly endDate) 
		{ 
			DateOnly now = DateOnly.FromDateTime(DateTime.Now);

			return now >= startDate && now <= endDate; 
		}

		private bool IsThisRoomAvailibleNow(Room room)
		{
			if (room.Bookings.Any()) 
			{
				foreach (var booking in room.Bookings)
				{
					if(IsCurrentDateInRange(booking.StartDate, booking.EndDate))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
