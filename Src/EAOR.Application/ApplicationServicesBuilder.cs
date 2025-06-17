using EAOR.Application.Contracts.Services;
using EAOR.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EAOR.Application
{
	public static class ApplicationServicesBuilder
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IOrderService, OrderService>();

			return services;
		}
	}
}
