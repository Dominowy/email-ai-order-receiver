﻿using EAOR.Application.Common.Models.Events;
using EAOR.Application.Contracts.Configuration;
using EAOR.Application.Contracts.Infrastructure.Services;
using EAOR.Infrastructure.BackgroundServices;
using MailKit;
using MailKit.Net.Imap;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EAOR.Infrastructure.Services
{
    public class ListenerService : IListenerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IImapSettings _imapSettings;
        private readonly ILogger<ListenerService> _logger;

        public ListenerService(
            IServiceScopeFactory scopeFactory,
            IImapSettings imapSettings,
            ILogger<ListenerService> logger)
        {
            _scopeFactory = scopeFactory;
            _imapSettings = imapSettings;
            _logger = logger;
        }

        public async Task StartListening(CancellationToken cancellationToken)
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
					var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

					_logger.LogInformation("New email detected in inbox");

                    try
                    {
						await mediator.Publish(new NewEmailFetchedEvent(), cancellationToken);

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
