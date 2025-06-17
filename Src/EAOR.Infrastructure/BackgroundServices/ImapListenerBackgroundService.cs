using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Hosting;

namespace EAOR.Infrastructure.BackgroundServices
{
    public class ImapListenerBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var client = new ImapClient();

            await client.ConnectAsync("imap.example.com", 993, true, stoppingToken);
            await client.AuthenticateAsync("mail@example.com", "password", stoppingToken);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly, stoppingToken);

            inbox.CountChanged += async (s, e) =>
            {
                Console.WriteLine("Nowy mail!");
                // Możesz wywołać EmailService.FetchAndSaveOrdersAsync()
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                await client.IdleAsync(stoppingToken);
            }

            await client.DisconnectAsync(true, stoppingToken);
        }
    }
}