namespace EAOR.Application.Contracts.Infrastructure.Configuration
{
    public interface ILlmSettings
    {
        string ApiKey { get; }
        string Url { get; }
        string Model { get; }
    }
}
