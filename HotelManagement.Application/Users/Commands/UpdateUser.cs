using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Users.Commands
{
	public record UpdateUser : IRequest
	{
		public Guid Id { get; init; }
		public string FirstName { get; init; }
		public string LastName { get; init; }
		public string Email { get; init; }
	}

	public class UpdateUserHandler : IRequestHandler<UpdateUser>
	{
		private readonly IApplicationDbContext _context;

		public UpdateUserHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateUser request, CancellationToken cancellationToken)
		{
			var entity = await _context.Users.FindAsync([request.Id] ,cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			entity.FirstName = request.FirstName;
			entity.LastName = request.LastName;
			entity.Email = request.Email;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
