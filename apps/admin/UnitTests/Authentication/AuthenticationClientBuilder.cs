using Authentication;
using System.Net;
using UnitTests.Utilities;

namespace UnitTests.Authentication;

internal class AuthenticationClientBuilder
{
    private HttpStatusCode _statusCode;
    private object? _responseContent;

    public AuthenticationClientBuilder WithResponseHavingStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;

        return this;
    }

    public AuthenticationClientBuilder WithResponseHavingContent(object content)
    {
        _responseContent = content;

        return this;
    }

    public AuthenticationClient Build()
    {
        FakeHttpMessageHandler fakeHttpMessageHandler = _responseContent is not null ? new(_statusCode, _responseContent) : new(_statusCode);
        var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://microshop.local") };
        return new(httpClient);
    }
}
