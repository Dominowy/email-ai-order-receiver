using EAOR.Application.Contracts.Infrastructure.Configuration;

namespace EAOR.Infrastructure.Configuration
{
    public class LlmSettings : ILlmSettings
    {
        public string ApiKey { get; set; } = "";
        public string Url { get; set; } = "";
        public string Model { get; set; } = "";
    }
}
