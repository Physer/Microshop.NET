using Authentication;
using System.Net;
using UnitTests.Utilities;

namespace UnitTests.Authentication;

internal class AuthenticationClientBuilder
{
    private HttpStatusCode _statusCode;

    public AuthenticationClientBuilder WithRequestsReturningStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;

        return this;
    }

    public AuthenticationClient Build()
    {
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(_statusCode);
        var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://microshop.local") };
        return new(httpClient);
    }
}
