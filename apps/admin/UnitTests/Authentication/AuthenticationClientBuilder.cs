using Authentication;
using System.Net;
using UnitTests.Utilities;

namespace UnitTests.Authentication;

internal class AuthenticationClientBuilder
{
    private HttpStatusCode _statusCode;
    private string? _responseContent;

    public AuthenticationClientBuilder WithResponseHavingStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;

        return this;
    }

    public AuthenticationClientBuilder WithResponseHavingContent(string serializedContent)
    {
        _responseContent = serializedContent;

        return this;
    }

    public AuthenticationClient Build()
    {
        FakeHttpMessageHandler fakeHttpMessageHandler = string.IsNullOrWhiteSpace(_responseContent) ? new(_statusCode) : new(_statusCode, _responseContent);
        var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://microshop.local") };
        return new(httpClient);
    }
}
