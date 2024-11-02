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

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUser command)
		{
			try
			{
				var userId = await _mediator.Send(command);

				if (userId == Guid.Empty) 
				{
					return BadRequest("An arror occured!");
				}

				return Ok(userId);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteUser(Guid id)
		{
			var command = new DeleteUser(id);

			await _mediator.Send(command);

			return NoContent();
		}
	}
}
