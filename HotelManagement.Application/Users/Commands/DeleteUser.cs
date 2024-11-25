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
			try
			{
				var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

				_context.Users.Remove(entity);
			}
			catch (Exception ex)
			{
				throw new Exception($"User with id = {request.Id} was not found.");
			}

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
