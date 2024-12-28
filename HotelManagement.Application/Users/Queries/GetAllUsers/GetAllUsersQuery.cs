using MediatR;

namespace HotelManagement.Application.Users.Queries.GetAllUsers
{
	public record class GetAllUsersQuery : IRequest<List<UserDTO>>
	{
	}	
}