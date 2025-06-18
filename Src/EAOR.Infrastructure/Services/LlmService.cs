using EAOR.Application.Contracts.Infrastructure.Configuration;
using EAOR.Application.Contracts.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EAOR.Infrastructure.Services
{
    public class LlmService : ILlmService
    {
        private readonly HttpClient _httpClient;
        private readonly ILlmSettings _settings;
        private readonly ILogger<LlmService> _logger;

        public LlmService(HttpClient httpClient, ILlmSettings settings, ILogger<LlmService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        }

        public async Task<string> Get(string systemPrompt, string userPrompt, CancellationToken cancellationToken)
        {
            var requestBody = new
            {
                model = _settings.Model,
                messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
            };

            var json = JsonSerializer.Serialize(requestBody);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_settings.Url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError($"API error: {response.StatusCode} - {error}");
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }

}
