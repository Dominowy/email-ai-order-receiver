using EAOR.Application.Common.Models.Events;
using EAOR.Application.Contracts.Configuration;
using EAOR.Application.Contracts.Infrastructure.Services;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EAOR.Infrastructure.Services
{
	public class EmailProcessingService : IEmailProcessingService
	{
        private readonly IMediator _mediator;
        private readonly IImapSettings _imapSettings;
        private readonly ILogger<EmailProcessingService> _logger;

        public EmailProcessingService(IMediator mediator, IImapSettings imapSettings, ILogger<EmailProcessingService> logger)
        {
            _mediator = mediator;
            _imapSettings = imapSettings;
            _logger = logger;
        }

        public async Task FetchMail(CancellationToken cancellationToken)
		{
            using var client = new ImapClient();

            await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl, cancellationToken);
            await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password, cancellationToken);

            var inbox = client.Inbox;
			await inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

			var unseen = await inbox.SearchAsync(SearchQuery.NotSeen, cancellationToken);

			_logger.LogInformation("Found {Count} unseen emails", unseen.Count);

			foreach (var uid in unseen)
			{
				try
				{
					var message = await inbox.GetMessageAsync(uid, cancellationToken);
					_logger.LogInformation("Processing email UID: {Uid}, Subject: {Subject}", uid, message.Subject);

                    using var ms = new MemoryStream();
                    message.WriteTo(ms);

                    await _mediator.Publish(new NewEmailFetchedEvent
                    {
                        Body = message.TextBody,
                        RawData = ms.ToArray(),
                        ReceivedDate = message.Date.DateTime
                    }, cancellationToken);

                    await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
					_logger.LogInformation("Saved email and marked as seen: UID {Uid}", uid);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Failed to process email with UID {Uid}", uid);
				}
			}

            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
