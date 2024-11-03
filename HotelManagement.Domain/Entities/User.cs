using HotelManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Domain.Entities
{
	public class User : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
	}
}
