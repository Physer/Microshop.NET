using Application.Exceptions;
using Authentication.Models;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace UnitTests.Authentication;

public class AuthenticationClientTests
{
    private readonly string _defaultUsername;
    private readonly string _defaultPassword;
    private readonly JsonSerializerOptions _defaultJsonSerializerOptions;

    public AuthenticationClientTests()
    {
        _defaultUsername = "microshop_user";
        _defaultPassword = "secure_password";
        _defaultJsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task SignInAsync_WithInvalidResponse_ThrowAuthenticationException(HttpStatusCode statusCode)
    {
        // Arrange
        var expectedErrorMesssage = "Invalid response received from the authentication service";
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(statusCode)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMesssage);
    }

    [Fact]
    public async Task SignInAsync_WithValidReponse_UnableToParse_ThrowsAuthenticationException()
    {
        // Arrange
        var expectedErrorMesssage = "Unable to determine the authentication service's status result";
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMesssage);
    }

    [Fact]
    public async Task SignInAsync_WithoutAccessTokenHeader_ThrowsAuthenticationException()
    {
        // Arrange
        var expectedErrorMesssage = "Unable to retrieve the access token";
        AuthenticationResponse responseBody = new("OK", new());
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(responseBody)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMesssage);
    }
}
