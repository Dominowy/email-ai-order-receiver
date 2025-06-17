using MailKit.Net.Imap;

namespace EAOR.Infrastructure.Services
{
	public interface IEmailService
	{
		Task FetchAndSaveOrdersAsync(ImapClient client, CancellationToken cancellationToken);
	}
}