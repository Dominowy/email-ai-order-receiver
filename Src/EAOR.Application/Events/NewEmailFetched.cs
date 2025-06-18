using EAOR.Application.Common.Models.Dtos;
using EAOR.Application.Common.Models.Events;
using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Contracts.Infrastructure.Services;
using MediatR;
using System.Text.Json;

namespace EAOR.Application.Events
{
    public class NewEmailFetchedEventHandler : INotificationHandler<NewEmailFetchedEvent>
    {
        private readonly IEmailProccessingService _emailService;
        private readonly ILlmService _llmService;
		private readonly IOrderService _orderService;

		public NewEmailFetchedEventHandler(IEmailProccessingService emailService, ILlmService llmService, IOrderService orderService)
        {
            _emailService = emailService;
            _llmService = llmService;
            _orderService = orderService;
        }

        public async Task Handle(NewEmailFetchedEvent notification, CancellationToken cancellationToken)
        {
            var unseenEmails = await _emailService.GetAllUnseenEmails(cancellationToken);

			foreach (var unseenEmail in unseenEmails)
			{
                var email = await _emailService.AddUnseenEmail(unseenEmail, cancellationToken);

				var systemPrompt = "Jesteś parserem zamówień z e-maili. Wyciągaj tylko dane w formacie JSON.";

				var userPrompt = $"Z wiadomości poniżej wyciągnij nazwę produktu, ilość i cenę. " +
                    $"W JSON właściwości to nazwa produktu - ProductName," +
                    $" ilość - Quantity a cena - Price. W przypadku ceny rozdziel cene od waulty i zwróć ją osobna właściwość Currency. " +
                    $"Zwróć to jako liste. Wiadomość:\n\n{email.Body}";

				var content = await _llmService.Get(systemPrompt, userPrompt, cancellationToken);

				var order = JsonSerializer.Deserialize<List<OrderDto>>(content);

                await _orderService.AddOrders(order, cancellationToken);

				await _emailService.MarkedMailToSeen(unseenEmail, cancellationToken);
			}
        }
    }
}
