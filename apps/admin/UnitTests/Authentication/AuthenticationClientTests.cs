using Application.Exceptions;
using FluentAssertions;
using System.Net;
using Xunit;

namespace UnitTests.Authentication;

public class AuthenticationClientTests
{
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
            .WithRequestsReturningStatusCode(statusCode)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync("username", "password"));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<AuthenticationException>();
    }
}
