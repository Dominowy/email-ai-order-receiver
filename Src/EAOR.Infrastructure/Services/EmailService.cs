using EAOR.Application.Contracts.Context;
using EAOR.Domain.Emails;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;

namespace EAOR.Infrastructure.Services
{
	public class EmailService : IEmailService
	{
		private readonly IApplicationDbContext _dbContext;
		private readonly ILogger<EmailService> _logger;

		public EmailService(IApplicationDbContext dbContext, ILogger<EmailService> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task FetchAndSaveOrdersAsync(ImapClient client, CancellationToken cancellationToken)
		{
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

					var email = new Email(message.HtmlBody ?? message.TextBody, message.Date.DateTime);
					await _dbContext.Set<Email>().AddAsync(email, cancellationToken);

					using var ms = new MemoryStream();
					message.WriteTo(ms);

					var emlFile = new EmailFile(
						$"mail-{email.Id}.eml",
						"message/rfc822",
						ms.ToArray(),
						email.Id);

					await _dbContext.Set<EmailFile>().AddAsync(emlFile, cancellationToken);
					await _dbContext.SaveChangesAsync(cancellationToken);

					await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
					_logger.LogInformation("Saved email and marked as seen: UID {Uid}", uid);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Failed to process email with UID {Uid}", uid);
				}
			}
		}
	}
}
