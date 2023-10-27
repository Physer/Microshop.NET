using System.Text.Json;

namespace UnitTests.Utilities;

/// <summary>
/// A fake HTTP message handler for injecting your own HTTP status code as a response when mocking the .NET HTTP Client
/// </summary>
internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly IEnumerable<FakeHttpMessage?> _httpMessages;
    private readonly JsonSerializerOptions _serializerOptions;

    public FakeHttpMessageHandler(IEnumerable<FakeHttpMessage?> httpMessages)
    {
        _httpMessages = httpMessages;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpMessages = _httpMessages.ToList();
        FakeHttpMessage? currentHttpMessage;
        if (httpMessages.Count > 1)
            currentHttpMessage = httpMessages.FirstOrDefault(message => message?.RequestUrl?.Equals(request?.RequestUri?.PathAndQuery) == true);
        else
            currentHttpMessage = httpMessages.First();

        if (currentHttpMessage is null)
            throw new FakeHttpMessageException($"Unable to locate a HTTP message for the URI: {request.RequestUri}");

        var serializedContent = JsonSerializer.Serialize(currentHttpMessage.Content, _serializerOptions);
        HttpResponseMessage message = new(currentHttpMessage.StatusCode)
        {
            Content = new StringContent(serializedContent)
        };

        if (currentHttpMessage?.Headers?.Any() == true)
        {
            foreach (var header in currentHttpMessage.Headers)
                message.Headers.Add(header.Key, header.Value);
        }

        return Task.FromResult(message);
    }
}
