using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Queries
{
	public record class GetAllUsers : IRequest<List<UserDTO>>
	{
	}

	public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllUsersHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<UserDTO>> Handle(GetAllUsers request, CancellationToken cancellationToken)
		{
			return await _context.Users.Select(u => new UserDTO
			{
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Email = u.Email,
				PhoneNumber = u.PhoneNumber,
			}).ToListAsync(cancellationToken);
		}
	}
}
