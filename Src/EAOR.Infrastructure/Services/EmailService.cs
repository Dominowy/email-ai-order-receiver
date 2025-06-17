using MailKit.Net.Imap;

namespace EAOR.Infrastructure.Services
{
	public class EmailService
	{
		public EmailService()
		{
			
		}

		internal static async Task FetchAndSaveOrdersAsync(ImapClient client, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
