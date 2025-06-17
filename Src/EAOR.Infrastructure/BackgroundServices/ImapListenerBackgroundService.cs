using EAOR.Infrastructure.Services;
using EAOR.Infrastructure.Settings;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EAOR.Infrastructure.BackgroundServices
{
	public class ImapListenerBackgroundService : BackgroundService
	{
		private readonly IEmailService _emailService;
		private readonly IImapSettings _imapSettings;
		private readonly ILogger<ImapListenerBackgroundService> _logger;

		public ImapListenerBackgroundService(IEmailService emailService, ILogger<ImapListenerBackgroundService> logger)
		{
			_emailService = emailService;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("IMAP listener started");

			using var client = new ImapClient();

			try
			{
				await client.ConnectAsync("imap.example.com", 993, true, cancellationToken);
				await client.AuthenticateAsync("mail@example.com", "password", cancellationToken);
				_logger.LogInformation("IMAP connection established");

				var inbox = client.Inbox;
				await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);

				inbox.CountChanged += async (s, e) =>
				{
					_logger.LogInformation("New email detected in inbox");

					try
					{
						await _emailService.FetchAndSaveOrdersAsync(client, cancellationToken);
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