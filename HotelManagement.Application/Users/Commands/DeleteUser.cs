using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Users.Commands
{
	public record DeleteUser (Guid Id) : IRequest;

	public class DeleteUserHandler : IRequestHandler<DeleteUser>
	{
		private readonly IApplicationDbContext _context;		

		public DeleteUserHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteUser request, CancellationToken cancellationToken)
		{
			var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			_context.Users.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
