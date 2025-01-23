using MediatR;

namespace HotelManagement.Application.Users.Commands.UpdateUser
{
	public record UpdateUserCommand : IRequest
	{
		public Guid Id { get; init; }
		public string FirstName { get; init; }
		public string LastName { get; init; }
		public string PhoneNumber { get; init; }
	}	
}