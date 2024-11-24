using HotelManagement.Application.Common;
using MediatR;

namespace HotelManagement.Application.Rooms.Commands
{
	public record DeleteRoom (Guid Id) : IRequest;	

	public class DeleteRoomHandler : IRequestHandler<DeleteRoom>
	{
		private readonly IApplicationDbContext _context;

		public DeleteRoomHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task Handle(DeleteRoom request, CancellationToken cancellationToken)
		{
			try
			{
				var entity = await _context.Rooms.FindAsync([request.Id], cancellationToken);

				_context.Rooms.Remove(entity);
			}
			catch (Exception ex) 
			{
				throw new Exception($"Room with id = {request.Id} was not found.");
			}		

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
