﻿using HotelManagement.Application.Bookings.Commands.Create;
using HotelManagement.Application.Bookings.Commands.Delete;
using HotelManagement.Application.Bookings.Commands.Update;
using HotelManagement.Application.Bookings.Queries.GetAllBookings;
using HotelManagement.Application.Bookings.Queries.GetBookingDetails;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : Controller
	{
		private readonly IMediator _mediator;

		public BookingsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Get all bookings.
		/// </summary>
		/// <returns>A list of bookings.</returns>
		/// <responce code="200">Returns the list of bookings.</responce>		
		[HttpGet]
		public async Task<ActionResult<List<Booking>>> GetAllBookings()
		{
			var query = new GetAllBookingsQuery();
			var bookings = await _mediator.Send(query);

			if (bookings is null or [])
			{
				return Ok("There are no bookings in the database.");
			}

			return Ok(bookings);
		}

		/// <summary>
		/// Get booking details by Id.
		/// </summary>
		/// <param name="bookingId"></param>
		/// <returns>Booking details.</returns>
		/// <responce code="200">Returns booking details.</responce>
		/// <responce code="400">Returns message about problem.</responce>		
		[HttpGet("{bookingId:guid}")]
		public async Task<ActionResult> GetBookingDetails(Guid bookingId)
		{
			var query = new GetBookingDetailsQuery(bookingId);
			var result = await _mediator.Send(query);

			if (result is null)
			{
				return NotFound();
			}

			return Ok(result);
		}


		/// <summary>
		/// Creates new booking.
		/// </summary>
		/// <param name="command">The booking details.</param>
		/// <returns>The ID of the newly created booking.</returns>
		/// <responce code="200">Return booking Id.</responce>
		/// <recponce code="400">One or more errors occurred.</recponce>
		[Authorize(Roles = "Admin, Manager, User")]
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateBooking([FromBody] CreateBookingCommand command)
		{
			var bookingId = await _mediator.Send(command);

			if (bookingId == Guid.Empty)
			{
				return Ok("An arror occured!");
			}

			return Ok(bookingId);
		}

		/// <summary>
		/// Delete booking.
		/// </summary>
		/// <param name="id">Booking id.</param>
		/// <returns>OK.</returns>
		/// <response code="200">Returns OK</response>
		/// <response code="404">Entity with id not found</response>
		[Authorize(Roles = "Admin, Manager")]
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteBooking(Guid id)
		{
			var command = new DeleteBookingCommand(id);

			await _mediator.Send(command);

			return NoContent();
		}

		/// <summary>
		/// Update booking.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		/// <responce code="404">Booking Id in URL does not match with Id in command.</responce>
		/// <responce code="400">Entity id was not found.</responce>
		[Authorize(Roles = "Admin, Manager")]
		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingCommand command)
		{
			if (id != command.Id) //Вместо этой проверки добавить UpdateBookingRequest без id, далее здесь формировать команду
			{
				return Ok("Booking Id in URL does not match with Id in command");
			}

			await _mediator.Send(command);

			return NoContent();
		}
	}
}