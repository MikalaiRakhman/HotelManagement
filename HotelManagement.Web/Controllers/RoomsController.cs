using HotelManagement.Application.Bookings.Queries.GetBookingsByRoomId;
using HotelManagement.Application.Rooms.Commands.Create;
using HotelManagement.Application.Rooms.Commands.Delete;
using HotelManagement.Application.Rooms.Commands.Update;
using HotelManagement.Application.Rooms.Queries.GetAllRooms;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
    public class RoomsController : Controller
    {
        private readonly IMediator _mediator;

		public RoomsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Get all rooms.
		/// </summary>
		/// <returns>All rooms.</returns>
		/// <responce code="200">Return list of rooms</responce>		
		[HttpGet]
		public async Task<ActionResult<List<Room>>> GetAllRooms()
		{
			var query = new GetAllRoomsQuery();
			var rooms = await _mediator.Send(query);

			if (rooms is null or [])
			{
				return NotFound("No rooms found");
			}

			return Ok(rooms);
		}

		/// <summary>
		/// Get bookings by room id.
		/// </summary>
		/// <param name="roomId">Room Id.</param>
		/// <returns>List of bookings.</returns>
		/// <responce code="200">Return list of booking.</responce>
		/// <responce code="404">Not found.</responce>		
		[HttpGet("{roomId}/bookings")]
		public async Task<ActionResult> GetBookingsByRoomId(Guid roomId)
		{
			var query = new GetBookingsByRoomIdQuery(roomId);
			var result = await _mediator.Send(query);

			if (result is null or [])
			{
				return NotFound();
			}

			return Ok(result);
		}


		/// <summary>
		/// Create new room.
		/// </summary>
		/// <param name="command">The command containing room data</param>
		/// <returns>Room id</returns>
		/// <responce code="200">Create new room.</responce>
		/// <responce code="400">One or more errors have occured.</responce>
		[Authorize(Roles = "Admin, Manager")]
		[HttpPost]
		public async Task<ActionResult<Guid>> CreateRoom([FromBody] CreateRoomCommand command)
		{
			var roomId = await _mediator.Send(command);

			if (roomId == Guid.Empty) 
			{
				return BadRequest("An arror occured!");
			}

			return Ok(roomId);
		}


		/// <summary>
		/// Remove room
		/// </summary>
		/// <param name="id">Room id.</param>
		/// <returns>No content.</returns>
		/// <responce code="200">No content.</responce>
		/// <responce code="400">Room with id was not found.</responce>
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteRoom (Guid id)
		{			
			var command = new DeleteRoomCommand(id);

			await _mediator.Send(command);

			return NoContent();
		}


		/// <summary>
		/// Update room.
		/// </summary>
		/// <param name="id">Room id.</param>
		/// <param name="command">The command containing new room data.</param>
		/// <returns></returns>
		/// <responce code="400">One or more errors have occured.</responce>
		/// <responce code="204">Room udated.</responce>
		[Authorize(Roles = "Admin, Manager")]
		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
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