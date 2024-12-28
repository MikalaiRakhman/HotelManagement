using FluentValidation;
using HotelManagement.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Commands.UpdateUser
{
	public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
	{
		private readonly IApplicationDbContext _context;

		public UpdateUserValidator(IApplicationDbContext context)
		{
			_context = context;

			RuleFor(u => u.FirstName)
				.MaximumLength(30)
				.NotEmpty()
				.WithMessage("First name is required.");

			RuleFor(u => u.LastName)
				.MaximumLength(30)
				.NotEmpty()
				.WithMessage("Last name is required.");

			RuleFor(u => u.PhoneNumber)
			  .NotEmpty().WithMessage("Phone number is required.")
			  .Matches(@"^\+\d{1,3}\s?\d{1,14}$").WithMessage("Phone number must be in a valid international format.");
		}

		private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
		{
			return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
		}
	}
}