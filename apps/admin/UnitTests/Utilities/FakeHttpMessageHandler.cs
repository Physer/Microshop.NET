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
    /// <param name="statusCode">A custom HTTP status code to return from your response</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode)
    {
        _httpStatusCode = statusCode;
        _responseObject = new
        {
            Message = "Microshop.NET"
        };
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(new HttpResponseMessage(_httpStatusCode) { Content = new StringContent(JsonSerializer.Serialize(_responseObject, new JsonSerializerOptions(JsonSerializerDefaults.Web))) });
}
