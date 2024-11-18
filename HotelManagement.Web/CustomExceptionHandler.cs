using HotelManagement.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HotelManagement.Web
{
	public class CustomExceptionHandler : IExceptionHandler
	{
		private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

		public CustomExceptionHandler()
		{
			_exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
			{
				{typeof(Validation), HandleValidationException }
			};
		}

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var exceptionType = exception.GetType();

			if (_exceptionHandlers.ContainsKey(exceptionType)) 
			{
				await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
				return true;
			}

			return false;
		}

		private async Task HandleValidationException(HttpContext httpContext, Exception ex)
		{
			var exception = (Validation)ex;

			httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

			await httpContext.Response.WriteAsJsonAsync(new HttpValidationProblemDetails(exception.Errors)
			{
				Title = ex.Message,
				Status = StatusCodes.Status400BadRequest,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
			});
		}
	}
}
