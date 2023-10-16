using System.Net;
using System.Text.Json;

namespace UnitTests.Utilities;

/// <summary>
/// A fake HTTP message handler for injecting your own HTTP status code as a response when mocking the .NET HTTP Client
/// </summary>
internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly object _content;
    private readonly IEnumerable<KeyValuePair<string, string>> _headers;
    private readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    /// A simple fake HTTP response message returning an HTTP 200OK status
    /// </summary>
    public FakeHttpMessageHandler() : this(HttpStatusCode.OK, new(), Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode) : this(statusCode, new(), Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code and custom response body
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode, object content) : this(statusCode, content, Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code, custom response body and custom headers
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    /// <param name="headers">Custom response header</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode, object content, IEnumerable<KeyValuePair<string, string>> headers)
    {
        _statusCode = statusCode;
        _content = content;
        _headers = headers;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var serializedContent = JsonSerializer.Serialize(_content, _serializerOptions);
        HttpResponseMessage message = new(_statusCode)
        {
            Content = new StringContent(serializedContent)
        };

        foreach (var header in _headers)
            message.Headers.Add(header.Key, header.Value);

        return Task.FromResult(message);
    }
}
