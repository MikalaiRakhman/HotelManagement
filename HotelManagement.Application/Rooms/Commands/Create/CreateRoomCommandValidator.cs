﻿using FluentValidation;
using HotelManagement.Application.Common;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Domain.Enums;

namespace HotelManagement.Application.Rooms.Commands.Create
{
	public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
	{
		private readonly IApplicationDbContext _context;

		public CreateRoomCommandValidator(IApplicationDbContext context)
		{
			_context = context;

			RuleFor(r => r.RoomNumber)
				.NotEmpty()
				.WithMessage("Room number is required.")
				.GreaterThan(0)
				.WithMessage("Room number must be greater than 0.")
				.MustAsync(BeUniqueRoomNumber)
				.WithMessage("Room number is already exist. Room number should be unique.");

			RuleFor(r => r.RoomType)
				.NotEmpty()
				.WithMessage("Room type is required.")
				.IsInEnum()
				.NotEqual(RoomType.None)
				.WithMessage("Room type cannot be None (0).");

			RuleFor(r => r.PricePerNight)
				.NotEmpty()
				.WithMessage("Price is required.")
				.GreaterThan(0)
				.WithMessage("Price must be greater than 0.");
		}

		private async Task<bool> BeUniqueRoomNumber(int roomNumber, CancellationToken cancellationToken)
		{
			return !await _context.Rooms.AnyAsync(r => r.RoomNumber == roomNumber, cancellationToken);
		}
	}
}
