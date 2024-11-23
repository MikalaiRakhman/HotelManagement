using FluentValidation;
using HotelManagement.Application.Bookings.Commands;
using HotelManagement.Application.Common;

namespace HotelManagement.Application.Bookings.Validators
{
	public class CreateBookingValidator : AbstractValidator<CreateBooking>
	{
		public CreateBookingValidator()
		{
			RuleFor(b => b.UserId)
				.NotEmpty()
				.WithMessage("UserId is requared");

			RuleFor(b => b.RoomId)
				.NotEmpty()
				.WithMessage("RoomId is required!");

			RuleFor(b => b.StartDate)
				.LessThan(b => b.EndDate)
				.WithMessage("Start date must be earlier than end date.")
				.NotEmpty()
				.WithMessage("Start date is required.");
		}
	}
}
