using HotelManagement.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace HotelManagement.Web.Filters
{
	public class ExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var statusCode = context.Exception switch
			{
				ValidationException => (int)HttpStatusCode.BadRequest,
				KeyNotFoundException => (int)HttpStatusCode.NotFound,
				_ => (int)HttpStatusCode.InternalServerError,
			};

			var responce = new
			{
				StatusCode = statusCode,
				Message = context.Exception.Message,
				Errors = context.Exception is ValidationException validationException
				? validationException.Errors
				: null
			};

			context.Result = new JsonResult(responce)
			{
				StatusCode = statusCode
			};

			context.ExceptionHandled = true;
		}
	}
}
