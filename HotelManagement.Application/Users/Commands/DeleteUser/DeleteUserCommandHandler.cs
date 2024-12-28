using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Users.Commands.DeleteUser
{
	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
	{
		private readonly IApplicationDbContext _context;

		public DeleteUserCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			_context.Users.Remove(entity);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}