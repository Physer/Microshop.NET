using Application.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Xunit;
using AuthenticationOptions = Application.Options.AuthenticationOptions;

namespace UnitTests.Authentication;

public class MicroshopAuthenticationHandlerTests
{
    [Fact]
    public async Task AuthenticateAsync_WithoutJwks_ReturnsFailedAuthenticateResult()
    {
        // Arrange
        var authenticationOptions = new AuthenticationOptions
        {
            BaseUrl = "http://localhost",
            Issuer = "issuer",
            RelativeJwksEndpoint = "/jwks-endpoint-integration"
        };
        var authenticationHandler = new MicroshopAuthenticationHandlerBuilder()
            .WithAuthenticationOptions(authenticationOptions)
            .WithJwksEndpointReturningData(null)
            .Build();

        // Act
        var result = await authenticationHandler.AuthenticateAsync();

        // Assert
        result.Should().BeEquivalentTo(AuthenticateResult.Fail("Unable to retrieve the JSON Web Key Set from the Authentication service"));
    }
}
