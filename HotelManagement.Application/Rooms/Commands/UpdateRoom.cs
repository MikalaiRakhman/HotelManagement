using HotelManagement.Application.Common;
using HotelManagement.Domain.Enums;
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
			Guard.AgainstNull(entity, nameof(entity));

			entity.RoomNumber = request.RoomNumber;
			entity.RoomType = request.RoomType;
			entity.PricePerNight = request.PricePerNight;
			entity.IsAvailable = true;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
