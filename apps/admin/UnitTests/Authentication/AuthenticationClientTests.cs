using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using FluentAssertions;
using System.Net;
using Xunit;

namespace UnitTests.Authentication;

public class AuthenticationClientTests
{
    private readonly string _defaultUsername;
    private readonly string _defaultPassword;
    private readonly string _accessTokenHeaderKey;
    private readonly string _roleClaimKey;

    public AuthenticationClientTests()
    {
        _defaultUsername = "microshop_user";
        _defaultPassword = "secure_password";
        _accessTokenHeaderKey = "st-access-token";
        _roleClaimKey = "st-role";
    }

    [Theory]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task SignInAsync_WithInvalidResponse_ShouldThrowAuthenticationException(HttpStatusCode statusCode)
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
    public async Task SignInAsync_WithValidReponse_UnableToParse_ShouldThrowAuthenticationException()
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
    public async Task SignInAsync_WithoutAccessTokenHeader_ShouldThrowAuthenticationException()
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

    [Fact]
    public async Task SignInAsync_WithValidData_ShouldReturnAuthenticationData()
    {
        // Arrange
        var id = "1234";
        var emailAddress = "unittest@microshop.rocks";
        var role = "User";
        var accessToken = "access_token";

        AuthenticationData expectedAuthenticationData = new(emailAddress, new[] { role }, accessToken);

        AuthenticationResponse validResponseBody = new("OK", new(id, emailAddress, DateTime.UtcNow.Ticks));
        HashSet<KeyValuePair<string, string>> headers = new()
        {
            new(_accessTokenHeaderKey, accessToken),
        };

        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(validResponseBody)
            .WithResponseHavingHeaders(headers)
            .WithGetRolesReturning(new[] { role })
            .Build();

        // Act
        var authenticationData = await authenticationClient.SignInAsync(_defaultUsername, _defaultPassword);

        // Assert
        authenticationData.Should().BeEquivalentTo(expectedAuthenticationData);
    }

    [Fact]
    public async Task SignInAsync_WithMultipleAccessTokens_ShouldTakeFirstAccessToken()
    {
        // Arrange
        var id = "1234";
        var emailAddress = "unittest@microshop.rocks";
        var role = "User";
        var firstAccessToken = "first_token";
        var secondAccessToken = "second_token";

        AuthenticationData expectedAuthenticationData = new(emailAddress, new[] { role }, firstAccessToken);

        AuthenticationResponse validResponseBody = new("OK", new(id, emailAddress, DateTime.UtcNow.Ticks));
        HashSet<KeyValuePair<string, string>> headers = new()
        {
            new(_accessTokenHeaderKey, firstAccessToken),
            new(_accessTokenHeaderKey, secondAccessToken)
        };

        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(validResponseBody)
            .WithResponseHavingHeaders(headers)
            .WithGetRolesReturning(new[] { role })
            .Build();

        // Act
        var authenticationData = await authenticationClient.SignInAsync(_defaultUsername, _defaultPassword);

        // Assert
        authenticationData.Should().BeEquivalentTo(expectedAuthenticationData);
    }
}
