using HotelManagement.Application.Bookings.Commands;
using HotelManagement.Application.Bookings.Queries;
using HotelManagement.Application.Common;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

		[HttpGet("{bookingId:guid}")]
		public async Task<ActionResult> GetBookingDetails(Guid bookingId)
		{
			var query = new GetBookingDetails(bookingId);
			var result = await _mediator.Send(query);

			if (result is null) 
			{
				return NotFound();
			}

			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateBooking([FromBody] CreateBooking command)
		{
			var bookingId = await _mediator.Send(command);

			if (bookingId == Guid.Empty) 
			{
				return BadRequest("An arror occured!");
			}

			return Ok(bookingId);			
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteBooking(Guid id)
		{
			var command = new DeleteBooking(id);

			try
			{
				await _mediator.Send(command);

				return NoContent();
			}
			catch (Exception ex) 
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateBooking(Guid id, [FromBody] UpdateBooking command)
		{
			if (id != command.Id)
			{
				return BadRequest("Booking Id in URL does not match with Id in command");
			}
			
			await _mediator.Send(command);

			return NoContent();			
		}		
	}
}
