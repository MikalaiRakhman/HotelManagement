using FluentValidation;
using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Rooms.Commands.Update
{
	public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
	{
		private readonly IApplicationDbContext _context;

		public UpdateRoomCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Rooms.FindAsync(request.Id, cancellationToken);
			Guard.AgainstNull(entity, nameof(entity));

			if (request.RoomNumber != entity.RoomNumber) 
			{
				if (await BeUniqueRoomNumber(request.RoomNumber, cancellationToken))
				{
					throw new ValidationException("Room number already exists. Room number should be unique.");
				}
			}

			entity.RoomNumber = request.RoomNumber;
			entity.RoomType = request.RoomType;
			entity.PricePerNight = request.PricePerNight;
			entity.IsAvailable = true;

			await _context.SaveChangesAsync(cancellationToken);
		}

		private async Task<bool> BeUniqueRoomNumber(int roomNumber, CancellationToken cancellationToken)
		{
			return await _context.Rooms.AnyAsync(r => r.RoomNumber == roomNumber, cancellationToken);
		}
	}
}