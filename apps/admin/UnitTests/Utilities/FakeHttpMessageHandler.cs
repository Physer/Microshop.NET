using System.Net;
using System.Text.Json;

namespace UnitTests.Utilities;

/// <summary>
/// A fake HTTP message handler for injecting your own HTTP status code as a response when mocking the .NET HTTP Client
/// </summary>
internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _httpStatusCode;
    private readonly object _responseObject;

    /// <summary>
    /// The default Fake HTTP Message Handler will return a status code of OK and a response body
    /// </summary>
    public FakeHttpMessageHandler() : this(HttpStatusCode.OK) { }
    /// <summary>
    /// A Fake HTTP Message Handler with a custom status code in your HTTP response with an included response body
    /// </summary>
    /// <param name="statusCode">The HTTP status code to return in the HTTP response</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode) : this(statusCode, new { Message = "Microshop.NET " }) { }

    /// <summary>
    /// A Fake HTTP Message Handler with a custom status code and a custom response object
    /// </summary>
    /// <param name="statusCode">The HTTP status code to return in the HTTP response</param>
    /// <param name="responseObject">The message content (serailizable as a string) to return in the HTTP response</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode, object responseObject)
    {
        _httpStatusCode = statusCode;
        _responseObject = responseObject;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(new HttpResponseMessage(_httpStatusCode) { Content = new StringContent(JsonSerializer.Serialize(_responseObject, new JsonSerializerOptions(JsonSerializerDefaults.Web))) });
}
