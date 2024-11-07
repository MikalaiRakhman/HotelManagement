using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;

namespace HotelManagement.Application.Users.Commands
{
	public class CreateUser : IRequest<Guid>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }		
		public string Email { get; set; }
	}

	public class CreateUserHandler : IRequestHandler<CreateUser, Guid>
	{
		private readonly IApplicationDbContext _context;

		public CreateUserHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Guid> Handle (CreateUser request, CancellationToken cancellationToken)
		{
			var entity = new User
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				CreatedAt = DateTime.UtcNow,
				LastModifiedAt = DateTime.UtcNow,
				Bookings = new List<Booking>()
			};

			_context.Users.Add(entity);

			await _context.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
