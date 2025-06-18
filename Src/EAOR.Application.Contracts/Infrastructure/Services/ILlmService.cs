namespace EAOR.Application.Contracts.Infrastructure.Services
{
    public interface ILlmService
    {
        Task<string> Get(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
    }
}
