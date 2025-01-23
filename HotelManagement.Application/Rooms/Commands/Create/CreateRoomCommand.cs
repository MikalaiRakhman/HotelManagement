using HotelManagement.Domain.Enums;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Create
{
	public class CreateRoomCommand : IRequest<Guid>
	{
		public int RoomNumber { get; set; }
		public RoomType RoomType { get; set; }
		public int PricePerNight { get; set; }
	}	
}