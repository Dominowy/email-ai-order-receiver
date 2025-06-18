using EAOR.Application.Contracts.Infrastructure.Services;
using Microsoft.Extensions.Hosting;

namespace EAOR.Infrastructure.BackgroundServices
{
	public class ImapBackgroundService : BackgroundService
	{
        private readonly IListenerService _imapListenerService;

        public ImapBackgroundService(IListenerService imapListenerService)
        {
            _imapListenerService = imapListenerService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
            await _imapListenerService.StartListening(cancellationToken);
        }
	}
}