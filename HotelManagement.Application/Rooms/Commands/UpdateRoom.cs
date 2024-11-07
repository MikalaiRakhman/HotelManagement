﻿using HotelManagement.Application.Common;
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
		public bool IsAvailable { get; init; }
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
			var entity = await _context.Rooms.FindAsync([request.Id], cancellationToken);

			if (entity == null)
			{
				throw new Exception($"Entity with Id = {request.Id} was not found!");
			}

			entity.RoomNumber = request.RoomNumber;
			entity.RoomType = request.RoomType;
			entity.PricePerNight = request.PricePerNight;
			entity.IsAvailable = request.IsAvailable;
			entity.LastModifiedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
