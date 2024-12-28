using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Users.Commands.UpdateUser
{
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
	{
		private readonly IApplicationDbContext _context;

		public UpdateUserCommandHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

			Guard.AgainstNull(entity, nameof(entity));

			entity.FirstName = request.FirstName;
			entity.LastName = request.LastName;
			entity.PhoneNumber = request.PhoneNumber;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}