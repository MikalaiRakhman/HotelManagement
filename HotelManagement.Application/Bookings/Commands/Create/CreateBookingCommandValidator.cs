using FluentValidation;

namespace HotelManagement.Application.Bookings.Commands.Create
{
	public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
	{
		public CreateBookingCommandValidator()
		{
			RuleFor(b => b.UserId)
				.NotEmpty()
				.WithMessage("User id is required.")
				.Must(IsValidGuid)
				.WithMessage("User id must be valid.");

			RuleFor(b => b.RoomId)
				.NotEmpty()
				.WithMessage("Room id is required.")
				.Must(IsValidGuid)
				.WithMessage("Room id must be valid.");

			RuleFor(b => b.StartDate)
				.LessThan(b => b.EndDate)
				.WithMessage("Start date must be earlier than end date.")
				.NotEmpty()
				.WithMessage("Start date is required.");
		}

		private bool IsValidGuid(Guid guid)
		{
			return guid != Guid.Empty;
		}
	}
}