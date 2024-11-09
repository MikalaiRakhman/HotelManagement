using FluentValidation;
using HotelManagement.Application.Users.Commands;

namespace HotelManagement.Application.Users.Validators
{
	public class UpdateUserValidator : AbstractValidator<UpdateUser>
	{
		public UpdateUserValidator()
		{
			RuleFor(u => u.FirstName)
				.MaximumLength(30)
				.NotEmpty()
				.WithMessage("FirstName is required!");

			RuleFor(u => u.LastName)
				.MaximumLength(30)
				.NotEmpty()
				.WithMessage("Lastname is required!");

			RuleFor(u => u.Email)
				.EmailAddress()
				.WithMessage("Email is required");
		}
	}
}
