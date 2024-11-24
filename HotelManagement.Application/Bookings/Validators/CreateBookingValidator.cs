using FluentValidation;
using HotelManagement.Application.Bookings.Commands;

namespace HotelManagement.Application.Bookings.Validators
{
	public class CreateBookingValidator : AbstractValidator<CreateBooking>
	{
		public CreateBookingValidator()
		{
			RuleFor(b => b.UserId)
				.NotEmpty()
				.WithMessage("UserId is required")
				.Must(IsValidGuid)
				.WithMessage("UserId must be a valid GUID");

			RuleFor(b => b.RoomId)
				.NotEmpty()
				.WithMessage("RoomId is required!")
				.Must(IsValidGuid)
				.WithMessage("RoomId must be a valid GUID");

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

