using EAOR.Application.Contracts.Configuration;
using EAOR.Application.Contracts.Infrastructure.Configuration;
using EAOR.Application.Contracts.Infrastructure.Context;
using EAOR.Application.Contracts.Infrastructure.Services;
using EAOR.Infrastructure.BackgroundServices;
using EAOR.Infrastructure.Configuration;
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

            var parserSettings = new LlmSettings();
            configuration.GetSection("ParserSettings").Bind(parserSettings);
            services.AddSingleton<ILlmSettings>(parserSettings);

            services.AddSingleton<IListenerService, ListenerService>();

            services.AddScoped<IEmailProcessingService, EmailProcessingService>();
            services.AddScoped<ILlmService, LlmService>();

            services.AddHostedService<ImapBackgroundService>();

			return services;
        }
    }
}
