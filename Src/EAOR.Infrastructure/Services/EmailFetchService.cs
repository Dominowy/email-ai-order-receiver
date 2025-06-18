using EAOR.Application.Common.Models.Identifier;
using EAOR.Application.Common.Models.Message;
using EAOR.Application.Contracts.Configuration;
using EAOR.Application.Contracts.Infrastructure.Services;
using EAOR.Infrastructure.Extension;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text;

namespace EAOR.Infrastructure.Services
{
	public class EmailFetchService : IEmailFetchService
	{
		private readonly IImapSettings _imapSettings;
		private readonly ILogger<EmailFetchService> _logger;

		public EmailFetchService(IImapSettings imapSettings, ILogger<EmailFetchService> logger)
		{
			_imapSettings = imapSettings;
			_logger = logger;
		}

		public async Task<List<Identifier>> FetchUnseenMail(CancellationToken cancellationToken)
		{
			using var client = new ImapClient();

			await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl, cancellationToken);
			await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password, cancellationToken);

			var inbox = client.Inbox;
			await inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

			var unseen = await inbox.SearchAsync(SearchQuery.NotSeen, cancellationToken);

			_logger.LogInformation("Found {Count} unseen emails", unseen.Count);

			await client.DisconnectAsync(true, cancellationToken);

			return [.. unseen.Select(uid => uid.ToEmailIdentifier())];
		}

		public async Task<EmailMessage> FetchMail(Identifier identifier, CancellationToken cancellationToken)
		{
			var uid = identifier.ToUniqueId();

			using var client = new ImapClient();

			await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl, cancellationToken);
			await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password, cancellationToken);

			var inbox = client.Inbox;

			await inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

			try
			{
				var message = await inbox.GetMessageAsync(uid, cancellationToken);
				_logger.LogInformation("Processing email UID: {Uid}, Subject: {Subject}", uid, message.Subject);

				using var ms = new MemoryStream();
				message.WriteTo(ms);

				string decodedBody = null;
				decodedBody = await BodyCovnerter(message, decodedBody);

				return new EmailMessage
				{
					Body = decodedBody,
					Data = ms.ToArray(),
					ReceivedDate = message.Date.DateTime,
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process email with UID {Uid}", uid);
				return null;
			}
			finally
			{
				await client.DisconnectAsync(true, cancellationToken);
			}
		}

		private static async Task<string> BodyCovnerter(MimeMessage message, string decodedBody)
		{
			var textPart = message.BodyParts.OfType<TextPart>()
								.FirstOrDefault(tp => tp.IsPlain || tp.IsHtml);

			if (textPart != null)
			{
				var charset = textPart.ContentType?.Charset;

				var encoding = !string.IsNullOrWhiteSpace(charset)
					? Encoding.GetEncoding(charset)
					: Encoding.UTF8;

				using var textStream = new MemoryStream();
				await textPart.Content.DecodeToAsync(textStream);
				decodedBody = encoding.GetString(textStream.ToArray());
			}
			else
			{
				decodedBody = message.TextBody;
			}

			return decodedBody;
		}

		public async Task MarkedSeenEmail(Identifier identifier, CancellationToken cancellationToken)
		{
			var uid = identifier.ToUniqueId();

			using var client = new ImapClient();

			await client.ConnectAsync(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl, cancellationToken);
			await client.AuthenticateAsync(_imapSettings.Username, _imapSettings.Password, cancellationToken);

			var inbox = client.Inbox;

			await inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

			try
			{
				await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
				_logger.LogInformation("Saved email and marked as seen: UID {Uid}", uid);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process email with UID {Uid}", uid);
			}
			finally
			{
				await client.DisconnectAsync(true, cancellationToken);
			}
		}
	}
}