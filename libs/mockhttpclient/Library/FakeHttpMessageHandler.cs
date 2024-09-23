using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("Tests")]
namespace Microshop.Library;

/// <summary>
/// A fake HTTP message handler for injecting your own HTTP messages as a response when mocking the .NET HTTP Client
/// </summary>
/// <remarks>
/// Create a fake HTTP message handler with one or more fake HTTP messages
/// </remarks>
/// <param name="httpMessages">A collection of fake HTTP messages to return a response for</param>
internal class FakeHttpMessageHandler(IEnumerable<FakeHttpMessage> httpMessages) : HttpMessageHandler
{
    private readonly IEnumerable<FakeHttpMessage> _httpMessages = httpMessages;
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var enumeratedHttpMessages = _httpMessages.ToList();
        FakeHttpMessage? currentHttpMessage = (enumeratedHttpMessages.Count > 1
            ? enumeratedHttpMessages.Find(message => message.RequestUrl?.Equals(request.RequestUri?.ToString()) == true)
            : enumeratedHttpMessages.FirstOrDefault())
            ?? throw new FakeHttpMessageException($"Unable to locate a HTTP message for the URI: {request?.RequestUri}");

        var serializedContent = JsonSerializer.Serialize(currentHttpMessage.Content, _serializerOptions);
        HttpResponseMessage message = new(currentHttpMessage.StatusCode)
        {
            Content = new StringContent(serializedContent),
            RequestMessage = request
        };

        foreach (var header in currentHttpMessage.Headers)
            message.Headers.Add(header.Key, header.Value);

        return Task.FromResult(message);
    }
}
