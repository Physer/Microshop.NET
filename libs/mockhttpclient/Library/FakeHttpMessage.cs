using System.Net;

namespace Microshop.MockHttpClient;

/// <summary>
/// A fake HTTP message, to be used in conjuction with the fake HTTP message handler to mock custom HTTP responses in unit tests
/// </summary>
/// <remarks>
/// A customized fake HTTP response message with an HTTP status code, custom response body and custom headers
/// </remarks>
/// <param name="statusCode">Custom HTTP status code</param>
/// <param name="content">Custom response message</param>
/// <param name="headers">Custom response header</param>
public class FakeHttpMessage(HttpStatusCode statusCode, object content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;
    public object Content { get; init; } = content;
    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; init; } = headers;

    public string? RequestUrl { get; init; }
    public HttpMethod? HttpMethod { get; init; }

    /// <summary>
    /// A simple fake HTTP response message returning an HTTP 200OK status
    /// </summary>
    public FakeHttpMessage() : this(HttpStatusCode.OK, new(), []) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    public FakeHttpMessage(HttpStatusCode statusCode) : this(statusCode, new(), []) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code and custom response body
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    public FakeHttpMessage(HttpStatusCode statusCode, object content) : this(statusCode, content, []) { }
}
