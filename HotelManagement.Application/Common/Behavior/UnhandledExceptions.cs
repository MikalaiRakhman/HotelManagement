using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelManagement.Application.Common.Behavior
{
	class UnhandledExceptions<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
	{
		private readonly ILogger<TRequest> _logger;

		public UnhandledExceptions(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;

				_logger.LogError(ex, $"Unhandled error occured for request {requestName} {request}");

				throw;
			}
		}
	}
}
