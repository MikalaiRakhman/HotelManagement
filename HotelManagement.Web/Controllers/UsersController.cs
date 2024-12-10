using HotelManagement.Application.Bookings.Queries;
using HotelManagement.Application.Users.Commands;
using HotelManagement.Application.Users.Queries;
using HotelManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : Controller
	{
		private readonly IMediator _mediator;

		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Get all users.
		/// </summary>
		/// <returns>List of users.</returns>
		/// <responce code="200">Return list of users.</responce>
		[HttpGet]
		public async Task<ActionResult<List<User>>> GerAllUsers()
		{
			var query = new GetAllUsers();
			var users = await _mediator.Send(query);

			if (users is null or [])
			{
				return NotFound("No users found");
			}

			return Ok(users);
		}

		/// <summary>
		/// Get all bookings of specific user.
		/// </summary>
		/// <param name="userId">User id/</param>
		/// <returns>Get bookings of specific user/</returns>
		/// <responce code="404">Not found/</responce>
		/// <responce code="200">Bookings list/</responce>
		[HttpGet("{userId}/bookings")]
		public async Task<ActionResult> GetBookingsByUserId(Guid userId)
		{
			var query = new GetBookingsByUserId(userId);
			var result = await _mediator.Send(query);

			if(result is null or [])
			{
				return NotFound();
			}

			return Ok(result);
		}


		/// <summary>
		/// Remove user.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>OK.</returns>
		/// <responce code="200">No content.</responce>
		/// <response code="404">User with id was not found.</response>
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteUser(Guid id)
		{
			try
			{
				var command = new DeleteUser(id);

				await _mediator.Send(command);
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}

			return NoContent();
		}


		/// <summary>
		/// Update specific user.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <param name="command">User details.</param>
		/// <returns></returns>
		/// <responce code="200">No content.</responce>
		/// <responce code="400">One or more errors have occured.</responce>
		[HttpPut("{id:guid}")]
		public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUser command)
		{
			if (id != command.Id)
			{
				return BadRequest("User Id in URL does not match with Id in command");
			}
			
			await _mediator.Send(command);

			return NoContent();			
		}
	}
}
