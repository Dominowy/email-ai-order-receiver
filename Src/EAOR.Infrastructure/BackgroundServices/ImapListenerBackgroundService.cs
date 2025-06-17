using EAOR.Infrastructure.Services;
using EAOR.Infrastructure.Settings;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EAOR.Infrastructure.BackgroundServices
{
	public class ImapListenerBackgroundService : BackgroundService
	{
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IImapSettings _imapSettings;
        private readonly ILogger<ImapListenerBackgroundService> _logger;

        public ImapListenerBackgroundService(
            IServiceScopeFactory scopeFactory,
            IImapSettings imapSettings,
            ILogger<ImapListenerBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _imapSettings = imapSettings;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("IMAP listener started");

			using var client = new ImapClient();

			try
			{
                await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl, cancellationToken);
                await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password, cancellationToken);

                _logger.LogInformation("IMAP connection established");

				var inbox = client.Inbox;
				await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);

				inbox.CountChanged += async (s, e) =>
				{
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    _logger.LogInformation("New email detected in inbox");

					try
					{
						await emailService.FetchAndSaveOrdersAsync(client, cancellationToken);
						_logger.LogInformation("Email processed and saved");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Failed to fetch and save email");
					}
				};

				while (!cancellationToken.IsCancellationRequested)
				{
					await client.IdleAsync(cancellationToken);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error in IMAP listener");
			}
			finally
			{
				await client.DisconnectAsync(true, cancellationToken);
				_logger.LogInformation("IMAP connection closed");
			}
		}
	}
}