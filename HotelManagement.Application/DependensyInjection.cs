using FluentValidation;
using HotelManagement.Application.Common.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelManagement.Application
{
	public static class DependensyInjection 
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
		{
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Validation<,>));
			});

			return services;
		}
	}
}
