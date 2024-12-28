using MediatR;

namespace HotelManagement.Application.Users.Commands.DeleteUser
{
	public record DeleteUserCommand(Guid Id) : IRequest;
}