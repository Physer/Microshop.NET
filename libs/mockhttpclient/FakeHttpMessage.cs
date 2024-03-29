﻿using System.Net;

namespace Microshop.MockHttpClient;

/// <summary>
/// A fake HTTP message, to be used in conjuction with the fake HTTP message handler to mock custom HTTP responses in unit tests
/// </summary>
public class FakeHttpMessage
{
    public HttpStatusCode StatusCode { get; init; }
    public object Content { get; init; }
    public IEnumerable<KeyValuePair<string, string>> Headers { get; init; }

    public string? RequestUrl { get; init; }
    public HttpMethod? HttpMethod { get; init; }

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
    public FakeHttpMessage(HttpStatusCode statusCode, object content) : this(statusCode, content, Array.Empty<KeyValuePair<string, string>>()) { }

    /// <summary>
    /// A customized fake HTTP response message with an HTTP status code, custom response body and custom headers
    /// </summary>
    /// <param name="statusCode">Custom HTTP status code</param>
    /// <param name="content">Custom response message</param>
    /// <param name="headers">Custom response header</param>
    public FakeHttpMessage(HttpStatusCode statusCode, object content, IEnumerable<KeyValuePair<string, string>> headers)
    {
        StatusCode = statusCode;
        Content = content;
        Headers = headers;
    }
}
