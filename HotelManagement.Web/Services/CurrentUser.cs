using HotelManagement.Application.Common;
using System.Security.Claims;

namespace HotelManagement.Web.Services
{
	public class CurrentUser : IUser
	{
		private readonly IHttpContextAccessor _accessor;

		public CurrentUser(IHttpContextAccessor accessor)
		{
			_accessor = accessor;
		}

		public Guid GetCurrentUser()
		{
			var userIdClaim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

			return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) 
				? userId 
				: Guid.NewGuid();
		}
	}
}
