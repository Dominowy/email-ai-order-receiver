using EAOR.Application.Common.Models.Events;
using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Contracts.Infrastructure.Services;
using MediatR;

namespace EAOR.Application.Events
{
    public class NewEmailFetchedEventHandler : INotificationHandler<NewEmailFetchedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILlmService _llmService;


        public NewEmailFetchedEventHandler(IEmailService emailService, ILlmService llmService)
        {
            _emailService = emailService;
            _llmService = llmService;
        }

        public async Task Handle(NewEmailFetchedEvent notification, CancellationToken cancellationToken)
        {
            await _emailService.AddEmail(notification.Body, notification.RawData, notification.ReceivedDate, cancellationToken);

            var systemPrompt = "Jesteś parserem zamówień z e-maili. Wyciągaj tylko dane w formacie JSON.";

            var userPrompt = $"Z wiadomości poniżej wyciągnij nazwę produktu, ilość i cenę:\n\n{notification.Body}";

            var content = await _llmService.Get(systemPrompt, userPrompt, cancellationToken);
        }
    }
}
