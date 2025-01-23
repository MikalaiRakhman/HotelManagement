using HotelManagement.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Queries.GetAllUsers
{
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllUsersQueryHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
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