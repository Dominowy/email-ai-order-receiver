using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EAOR.Application
{
	public static class ApplicationServicesBuilder
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEmailProccessingService, EmailProccessingService>();

            return services;
		}
	}
}
