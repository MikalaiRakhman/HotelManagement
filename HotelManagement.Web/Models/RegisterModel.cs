using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Web.Models
{
	public record RegisterModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Email must be a valid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Firstname is requared.")]
		[StringLength(30, ErrorMessage = "First name cannot be longer than 50 characters.")]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Lastname is requared.")]
		[StringLength(30, ErrorMessage = "Last name cannot be longer than 50 characters.")]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Phone number is requared.")]
		[Phone(ErrorMessage = "Phone number is not valid.")]
		[RegularExpression(@"^\+\d{1,3}\s?\d{1,14}$", ErrorMessage = "Phone number must be in a valid international format.")]
		public string PhoneNumber { get; set; }
	}
}
