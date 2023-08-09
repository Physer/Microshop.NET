using System.Net;
using System.Text.Json;

namespace UnitTests;

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly object? _response;

    public FakeHttpMessageHandler(HttpStatusCode statusCode,
        object? response)
    {
        _statusCode = statusCode;
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult<HttpResponseMessage>(new()
    {
        StatusCode = _statusCode,
        Content = new StringContent(JsonSerializer.Serialize(_response))
    });
}
