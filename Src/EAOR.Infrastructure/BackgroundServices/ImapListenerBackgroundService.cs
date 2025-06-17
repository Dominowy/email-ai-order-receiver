using EAOR.Infrastructure.Services;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Hosting;

namespace EAOR.Infrastructure.BackgroundServices
{
    public class ImapListenerBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var client = new ImapClient();

            await client.ConnectAsync("imap.example.com", 993, true, cancellationToken);
            await client.AuthenticateAsync("mail@example.com", "password", cancellationToken);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);

			inbox.CountChanged += async (s, e) =>
            {
                Console.WriteLine("Nowy mail!");
                await EmailService.FetchAndSaveOrdersAsync(client, cancellationToken);
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                await client.IdleAsync(cancellationToken);
            }

            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}