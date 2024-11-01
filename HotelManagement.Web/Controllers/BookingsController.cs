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
	}
}
