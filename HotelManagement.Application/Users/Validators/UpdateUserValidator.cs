using FluentValidation;
using HotelManagement.Application.Common;
using HotelManagement.Application.Users.Commands;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Users.Validators
{
	public class UpdateUserValidator : AbstractValidator<UpdateUser>
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
		}

		private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
		{
			return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
		}
	}
}
