using FluentValidation;
using HotelManagement.Application.Rooms.Commands;

namespace HotelManagement.Application.Rooms.Validators
{
	public class UpdateRoomValidator : AbstractValidator<UpdateRoom>
	{
		public UpdateRoomValidator()
		{
			RuleFor(r => r.RoomNumber)
				.NotEmpty()
				.WithMessage("RoomNumber is required!")
				.GreaterThan(0)
				.WithMessage("RoomNumber must be greater than 0");

			RuleFor(r => r.RoomType)
				.NotEmpty()
				.WithMessage("RoomType is required!")
				.IsInEnum();

			RuleFor(r => r.PricePerNight)
				.NotEmpty()
				.WithMessage("PricePerNight is required!")
				.GreaterThan(0)
				.WithMessage("PricePerNight must be greater than 0");

			RuleFor(r => r.IsAvailable)
				.NotEmpty()
				.WithMessage("IsAvailable is requared!")
				.NotNull()
				.WithMessage("IsAvailable must be true or false");
		}
	}
}
