using MediatR;

namespace HotelManagement.Application.Rooms.Commands.Delete
{
	public record DeleteRoomCommand(Guid Id) : IRequest;	
}