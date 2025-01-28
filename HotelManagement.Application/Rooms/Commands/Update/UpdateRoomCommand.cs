using HotelManagement.Domain.Enums;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Update
{
	public record UpdateRoomCommand : IRequest
	{
		public Guid Id { get; init; }
		public int RoomNumber { get; init; }
		public RoomType RoomType { get; init; }
		public int PricePerNight { get; init; }
	}
}