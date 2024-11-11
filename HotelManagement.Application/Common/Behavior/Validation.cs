﻿using FluentValidation;
using MediatR;

namespace HotelManagement.Application.Common.Behavior
{
	public class Validation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public Validation(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators.Any()) 
			{
				var context = new ValidationContext<TRequest>(request);

				var validationResult = await Task.WhenAll(_validators
					.Select(v => v.ValidateAsync(context, cancellationToken)));

				var failures = validationResult.Where(r => r.Errors.Any())
					.SelectMany(r => r.Errors)
					.ToList();

				if (failures.Any())
				{
					throw new ValidationException(failures);
				}
			}

			return await next();
		}
	}
}
