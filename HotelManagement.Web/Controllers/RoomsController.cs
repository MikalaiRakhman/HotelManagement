using HotelManagement.Application.Bookings.Queries;
using HotelManagement.Application.Rooms.Commands;
using HotelManagement.Application.Rooms.Queries;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class RoomsController : Controller
    {
        private readonly IMediator _mediator;

		public RoomsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<List<Room>>> GetAllRooms()
		{
			var query = new GetAllRooms();
			var rooms = await _mediator.Send(query);

			if (rooms is null or [])
			{
				return NotFound("No rooms found");
			}

			return Ok(rooms);
		}

		[HttpGet("{roomId}/bookings")]
		public async Task<ActionResult> GetBookingsByRoomId(Guid roomId)
		{
			var query = new GetBookingsByRoomId(roomId);
			var resylt = await _mediator.Send(query);

			if (resylt is null or [])
			{
				return NotFound();
			}

			return Ok(resylt);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateRoom([FromBody] CreateRoom command)
		{
			var roomId = await _mediator.Send(command);

			if (roomId == Guid.Empty) 
			{
				return BadRequest("An arror occured!");
			}

			return Ok(roomId);			
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteRoom (Guid id)
		{
			var command = new DeleteRoom(id);

			await _mediator.Send(command);

			return NoContent();
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoom command)
		{
			if (id != command.Id)
			{
				return BadRequest("Room Id in URL does not match with Id in command");
			}

			await _mediator.Send(command);

			return NoContent();			
		}
	}
}
