using EAOR.Application.Contracts.Context;
using EAOR.Infrastructure.BackgroundServices;
using EAOR.Infrastructure.Context;
using EAOR.Infrastructure.Services;
using EAOR.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EAOR.Infrastructure
{
    public static class InfrastructureServicesBuilder
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            var imapSettings = new ImapSettings();
            configuration.GetSection("ImapSettings").Bind(imapSettings);

            services.AddSingleton<IImapSettings>(imapSettings);

            services.AddScoped<IEmailService, EmailService>();
			services.AddHostedService<ImapListenerBackgroundService>();

			return services;
        }
    }
}
