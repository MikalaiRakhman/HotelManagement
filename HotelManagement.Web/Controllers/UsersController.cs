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
	}
}
