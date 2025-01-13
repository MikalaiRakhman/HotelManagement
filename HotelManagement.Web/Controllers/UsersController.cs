using HotelManagement.Application.Bookings.Queries.GetBookingsByUserId;
using HotelManagement.Application.Common;
using HotelManagement.Application.Users.Commands.DeleteUser;
using HotelManagement.Application.Users.Commands.UpdateUser;
using HotelManagement.Application.Users.Queries.GetAllUsers;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Web.Controllers
{
	//[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : Controller
	{
		private readonly IMediator _mediator;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IApplicationDbContext _context;

		public UsersController(IMediator mediator, UserManager<ApplicationUser> userManager, IApplicationDbContext context)
		{
			_mediator = mediator;
			_userManager = userManager;
			_context = context;
		}

		/// <summary>
		/// Get all users.
		/// </summary>
		/// <returns>List of users.</returns>
		/// <responce code="200">Return list of users.</responce>
		[HttpGet]
		//[Authorize(Roles = "Admin")]
		public async Task<ActionResult<List<User>>> GetAllUsers()
		{
			var query = new GetAllUsersQuery();
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
		//[Authorize(Roles = "Admin, Manager")]
		[HttpGet("{userId}/bookings")]
		public async Task<ActionResult> GetBookingsByUserId(Guid userId)
		{
			var query = new GetBookingsByUserIdQuery(userId);
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
		//[Authorize(Roles = "Admin")]
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteUser(Guid id)
		{
			var user = await _context.Users.FindAsync(id);
			Guard.AgainstNull(user, nameof(user));

			var applicationUser = await _userManager.FindByEmailAsync(user.Email);
			Guard.AgainstNull(applicationUser, nameof(applicationUser));

			var command = new DeleteUserCommand(id);

			await _mediator.Send(command);
			await _userManager.DeleteAsync(applicationUser);

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
		//[Authorize(Roles = "Admin, Manager")]
		[HttpPut("{id:guid}")]		
		public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
		{
			if (id != command.Id)
			{
				return BadRequest("User Id in URL does not match with Id in command");
			}
			
			await _mediator.Send(command);
			
			var user = await _context.Users.FindAsync(id);
			Guard.AgainstNull(user, nameof(user));

			var userEmail = user.Email;
			var applicationUser = await _userManager.FindByEmailAsync(userEmail);
			Guard.AgainstNull(applicationUser, nameof(applicationUser));

			await _userManager.UpdateAsync(applicationUser);

			return NoContent();
		}
	}
}