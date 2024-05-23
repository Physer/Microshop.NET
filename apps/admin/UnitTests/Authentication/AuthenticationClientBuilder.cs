using Application.Authentication;
using Authentication;
using Microshop.MockHttpClient;
using NSubstitute;
using System.Net;

namespace UnitTests.Authentication;

internal class AuthenticationClientBuilder : HttpClientBuilder<AuthenticationClientBuilder>
{
    private readonly ITokenParser _tokenParser;

    public AuthenticationClientBuilder()
    {
        _statusCode = HttpStatusCode.OK;
        _responseContent = new();
        _headers = [];

        _tokenParser = Substitute.For<ITokenParser>();
    }

    public AuthenticationClientBuilder WithGetRolesReturning(IEnumerable<string> roles)
    {
        _tokenParser.GetRoles(Arg.Any<string>()).Returns(roles);

        return this;
    }

    public AuthenticationClient Build() => new(BuildHttpClient(), _tokenParser);
}
