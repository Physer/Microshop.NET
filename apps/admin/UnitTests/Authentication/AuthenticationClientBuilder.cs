using Application.Authentication;
using Authentication;
using NSubstitute;
using System.Net;
using UnitTests.Utilities;

namespace UnitTests.Authentication;

internal class AuthenticationClientBuilder
{
    private HttpStatusCode _statusCode;
    private object _responseContent;
    private IEnumerable<KeyValuePair<string, string>> _headers;

    private readonly ITokenParser _tokenParser;

    public AuthenticationClientBuilder()
    {
        _statusCode = HttpStatusCode.OK;
        _responseContent = new();
        _headers = Array.Empty<KeyValuePair<string, string>>();

        _tokenParser = Substitute.For<ITokenParser>();
    }

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

    public AuthenticationClientBuilder WithResponseHavingHeaders(IEnumerable<KeyValuePair<string, string>> headers)
    {
        _headers = headers;

        return this;
    }

    public AuthenticationClientBuilder WithGetRolesReturning(IEnumerable<string> roles)
    {
        _tokenParser.GetRoles(Arg.Any<string>()).Returns(roles);

        return this;
    }

    public AuthenticationClient Build()
    {
        FakeHttpMessageHandler fakeHttpMessageHandler = new(_statusCode, _responseContent, _headers);
        var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://microshop.local") };
        return new(httpClient, _tokenParser);
    }
}
