using System.Net.Http.Json;

namespace HomeMaintenanceScheduler.Api.Services;

public class DiscordNotifier
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public DiscordNotifier(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        var webhookUrl = _configuration["Discord:WebhookUrl"];

        if (string.IsNullOrWhiteSpace(webhookUrl))
            throw new InvalidOperationException("Discord webhook URL is not configured.");

        var client = _httpClientFactory.CreateClient();

        var payload = new
        {
            content = message
        };

        var response = await client.PostAsJsonAsync(webhookUrl, payload, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}