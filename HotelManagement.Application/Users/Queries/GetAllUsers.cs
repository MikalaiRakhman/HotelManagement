using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Queries
{
	public record class GetAllUsers : IRequest<List<UserDTO>>
	{
	}

	public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDTO>>
	{
		private readonly IApplicationDbContext _context;

		public GetAllUsersHandler(IApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<UserDTO>> Handle(GetAllUsers request, CancellationToken cancellationToken)
		{
			var usersDTO = new List<UserDTO>();
			var allUsers = await _context.Users.ToListAsync(cancellationToken);

			foreach (User user in allUsers)
			{
				var bookingsIdFromUser = new List<Guid>();

				if(user.Bookings != null)
				{
					foreach (Booking booking in user.Bookings)
					{
						bookingsIdFromUser.Add(booking.Id);
					}
				}				

				usersDTO.Add(new UserDTO
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					BookingsId = bookingsIdFromUser,
				});
			}

			return usersDTO;
		}
	}
}
