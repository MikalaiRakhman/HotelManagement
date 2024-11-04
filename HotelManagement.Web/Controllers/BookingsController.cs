using HotelManagement.Application.Bookings.Commands;
using HotelManagement.Application.Bookings.Queries;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : Controller
	{
		private readonly IMediator _mediator;

		public BookingsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<Booking>>> GetAllBookings()
		{
			var query = new GetAllBookings();
			var bookings = await _mediator.Send(query);

			if (bookings is null or [])
			{
				return NotFound("No bookings found!");
			}

			return Ok(bookings);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateBooking([FromBody] CreateBooking command)
		{
			try
			{
				var bookingId = await _mediator.Send(command);

				if (bookingId == Guid.Empty) 
				{
					return BadRequest("An arror occured!");
				}

				return Ok(bookingId);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteBooking(Guid id)
		{
			var command = new DeleteBooking(id);

			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateBooking(Guid id, [FromBody] UpdateBooking command)
		{
			if (id != command.Id)
			{
				return BadRequest("Booking Id in URL does not match with Id in command");
			}

			try
			{
				await _mediator.Send(command);

				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
