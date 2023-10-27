using System.Net;

namespace UnitTests.Utilities;

/// <summary>
/// A fake HTTP message, to be used in conjuction with the fake HTTP message handler to mock custom HTTP responses in unit tests
/// </summary>
internal class FakeHttpMessage
{
    public string? RequestUrl { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public object Content { get; init; }
    public IEnumerable<KeyValuePair<string, string>> Headers { get; init; }

    /// <summary>
    /// A simple fake HTTP response message returning an HTTP 200OK status
    /// </summary>
    public FakeHttpMessage() : this(HttpStatusCode.OK, new(), Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    public FakeHttpMessage(HttpStatusCode statusCode) : this(statusCode, new(), Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code and custom response body
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    public FakeHttpMessage(HttpStatusCode statusCode, object? content) : this(statusCode, content, Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code, custom response body and custom headers
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    /// <param name="headers">Custom response header</param>
    public FakeHttpMessage(HttpStatusCode statusCode, object? content, IEnumerable<KeyValuePair<string, string>>? headers) : this(statusCode, content, headers, string.Empty) { }

    /// <summary>
    /// A customized fake HTTP response message for a specific request URL with an HTTP status code, custom response body and custom headers
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    /// <param name="headers">Custom response header</param>
    /// <param name="requestUrl">The request URL for which this message applies</param>
    public FakeHttpMessage(HttpStatusCode statusCode, object content, IEnumerable<KeyValuePair<string, string>> headers, string requestUrl)
    {
        RequestUrl = requestUrl;
        StatusCode = statusCode;
        Content = content;
        Headers = headers;
    }
}
