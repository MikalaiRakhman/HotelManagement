using FluentValidation;
using HotelManagement.Application.Common;
using HotelManagement.Application.Rooms.Commands;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Rooms.Validators
{
	public class UpdateRoomValidator : AbstractValidator<UpdateRoom>
	{
		private readonly IApplicationDbContext _context;

		public UpdateRoomValidator(IApplicationDbContext context)
		{
			_context = context;

			RuleFor(r => r.RoomNumber)
				.NotEmpty()
				.WithMessage("RoomNumber is required!")
				.GreaterThan(0)
				.WithMessage("RoomNumber must be greater than 0")
				.MustAsync(BeUniqueRoomNumber)
				.WithMessage("RoomNumber is already exist. RoomNumber should be unique.");

			RuleFor(r => r.RoomType)
				.NotEmpty()
				.WithMessage("RoomType is required!")
				.IsInEnum();

			RuleFor(r => r.PricePerNight)
				.NotEmpty()
				.WithMessage("PricePerNight is required!")
				.GreaterThan(0)
				.WithMessage("PricePerNight must be greater than 0");
		}

		private async Task<bool> BeUniqueRoomNumber(int roomNumber, CancellationToken cancellationToken)
		{
			return !await _context.Rooms.AnyAsync(r => r.RoomNumber == roomNumber, cancellationToken);
		}
	}
}
