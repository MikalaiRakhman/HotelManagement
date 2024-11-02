using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Queries
{
	public record class GetAllUsers : IRequest<List<User>>
	{
	}

	public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<User>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllUsersHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<User>> Handle(GetAllUsers request, CancellationToken cancellationToken)
		{
			return await _context.Users
				.Select(u => new User
				{
					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Email = u.Email,
				}).ToListAsync(cancellationToken);
		}
	}
}
