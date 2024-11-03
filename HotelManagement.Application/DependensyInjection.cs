using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelManagement.Application
{
	public static class DependensyInjection 
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
		{
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			return services;
		}
	}
}
