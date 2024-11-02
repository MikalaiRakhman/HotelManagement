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

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateRoom([FromBody] CreateRoom command)
		{
			try
			{
				var roomId = await _mediator.Send(command);

				if (roomId == Guid.Empty) 
				{
					return BadRequest("An arror occured!");
				}

				return Ok(roomId);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteRoom (Guid id)
		{
			var command = new DeleteRoom(id);

			await _mediator.Send(command);

			return NoContent();
		}

	}
}
