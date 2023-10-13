using Application.Exceptions;
using FluentAssertions;
using System.Net;
using Xunit;

namespace UnitTests.Authentication;

public class AuthenticationClientTests
{
    private readonly string _defaultUsername;
    private readonly string _defaultPassword;

    public AuthenticationClientTests()
    {
        _defaultUsername = "microshop_user";
        _defaultPassword = "secure_password";
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
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(statusCode)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
    }

    [Fact]
    public async Task SignInAsync_WithValidReponse_UnableToParse_ThrowsAuthenticationException()
    {
        // Arrange
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
    }
}
