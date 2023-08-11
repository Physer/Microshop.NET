using API.Authentication;
using AutoFixture.Xunit2;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Xunit;
using AuthenticationOptions = Application.Options.AuthenticationOptions;

namespace UnitTests.Authentication;

public class MicroshopAuthenticationHandlerTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-key")]
    [InlineData(1)]
    [InlineData("{{ \"validJson\": \"yes\" }}")]
    public async Task AuthenticateAsync_WithoutJwks_ReturnsFailedAuthenticateResult(object? response)
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
            .WithJwksEndpointReturningData(response)
            .Build();

        // Act
        await authenticationHandler.InitializeAsync(new AuthenticationScheme(JwtBearerDefaults.AuthenticationScheme, null, typeof(MicroshopAuthenticationHandler)), new DefaultHttpContext());
        var result = await authenticationHandler.AuthenticateAsync();

        // Assert
        var failedAuthenticateResult = AuthenticateResult.Fail("Unable to retrieve the JWKS from the Authentication service");
        result.Should().BeEquivalentTo(failedAuthenticateResult);
    }

    [Theory]
    [InlineData("invald-uri")]
    [InlineData("")]
    [InlineData(" ")]
    public void AuthenticateAsync_WithInvalidBaseUrl_ThrowsUriFormatException(string baseUrl)
    {
        // Arrange
        var authenticationOptions = new AuthenticationOptions
        {
            BaseUrl = baseUrl,
            Issuer = string.Empty,
            RelativeJwksEndpoint = string.Empty
        };
        var authenticationHandlerBuilder = new MicroshopAuthenticationHandlerBuilder().WithAuthenticationOptions(authenticationOptions);

        // Act
        var exception = Record.Exception(authenticationHandlerBuilder.Build);

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<UriFormatException>();
    }

    [Theory]
    [AutoData]
    public async Task AuthenticateAsync_WithValidData_ReturnsNoResultAuthenticateResult(JwksResponseObject jwksResponse)
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
            .WithJwksEndpointReturningData(jwksResponse)
            .Build();

        // Act
        await authenticationHandler.InitializeAsync(new AuthenticationScheme(JwtBearerDefaults.AuthenticationScheme, null, typeof(MicroshopAuthenticationHandler)), new DefaultHttpContext());
        var result = await authenticationHandler.AuthenticateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(AuthenticateResult.NoResult());
    }
}
