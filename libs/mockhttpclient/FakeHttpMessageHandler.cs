using System.Text.Json;

namespace Microshop.MockHttpClient;

/// <summary>
/// A fake HTTP message handler for injecting your own HTTP messages as a response when mocking the .NET HTTP Client
/// </summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly IEnumerable<FakeHttpMessage?> _httpMessages;
    private readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    /// Create a fake HTTP message handler with one or more fake HTTP messages
    /// </summary>
    /// <param name="httpMessages">A collection of fake HTTP messages to return a response for</param>
    public FakeHttpMessageHandler(IEnumerable<FakeHttpMessage?> httpMessages)
    {
        _httpMessages = httpMessages;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpMessages = _httpMessages.ToList();
        FakeHttpMessage? currentHttpMessage = (httpMessages.Count > 1
            ? httpMessages.FirstOrDefault(message => message?.RequestUrl?.Equals(request?.RequestUri?.PathAndQuery) == true)
            : httpMessages.FirstOrDefault())
            ?? throw new FakeHttpMessageException($"Unable to locate a HTTP message for the URI: {request.RequestUri}");

        var serializedContent = JsonSerializer.Serialize(currentHttpMessage.Content, _serializerOptions);
        HttpResponseMessage message = new(currentHttpMessage.StatusCode)
        {
            Content = new StringContent(serializedContent),
            RequestMessage = request
        };

        foreach (var header in currentHttpMessage.Headers ?? Array.Empty<KeyValuePair<string, string>>())
            message.Headers.Add(header.Key, header.Value);

        return Task.FromResult(message);
    }
}
