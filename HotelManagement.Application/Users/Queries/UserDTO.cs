using HotelManagement.Domain.Common;

namespace HotelManagement.Application.Users.Queries
{
	public class UserDTO : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }		
	}
}
