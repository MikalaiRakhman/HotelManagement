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

			if (entity == null) 
			{
				throw new Exception($"Entity with Id = {request.Id} was not found!");
			}

			entity.FirstName = request.FirstName;
			entity.LastName = request.LastName;
			entity.Email = request.Email;
			entity.LastModifiedAt = DateTime.Now;

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
