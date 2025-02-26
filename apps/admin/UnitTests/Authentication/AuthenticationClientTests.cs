using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using AutoFixture.Xunit2;
using Microshop.MockHttpClient;
using Shouldly;
using System.Net;
using System.Text.Json;
using Xunit;

namespace UnitTests.Authentication;

public class AuthenticationClientTests
{
    private readonly string _defaultUsername;
    private readonly string _defaultPassword;
    private readonly string _accessTokenHeaderKey;

    public AuthenticationClientTests()
    {
        _defaultUsername = "microshop_user";
        _defaultPassword = "secure_password";
        _accessTokenHeaderKey = "st-access-token";
    }

    [Theory]
    [ClassData(typeof(InvalidHttpResponseClassData))]
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
        exception.ShouldBeOfType<AuthenticationException>();
        exception.Message.ShouldBeEquivalentTo(expectedErrorMesssage);
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
        exception.ShouldBeOfType<AuthenticationException>();
        exception.Message.ShouldBeEquivalentTo(expectedErrorMesssage);
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
        exception.ShouldBeOfType<AuthenticationException>();
        exception.Message.ShouldBeEquivalentTo(expectedErrorMesssage);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("username", "")]
    [InlineData("", "password")]
    [InlineData(" ", " ")]
    [InlineData(" ", "password")]
    [InlineData("username", " ")]
    public async Task SignInAsync_WithEmptyUsernameOrPassword_ShouldThrowAuthenticationException(string username, string password)
    {
        // Arrange
        var authenticationClient = new AuthenticationClientBuilder().Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(username, password));

        // Assert
        exception.ShouldBeOfType<AuthenticationException>();
    }

    [Theory]
    [AutoData]
    public async Task SignInAsync_WithInvalidResponse_ShouldThrowJsonException(string content)
    {
        // Arrange
        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(content)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => authenticationClient.SignInAsync(_defaultUsername, _defaultPassword));

        // Assert
        exception.ShouldBeOfType<JsonException>();
    }

    [Theory]
    [AutoData]
    public async Task SignInAsync_WithValidData_ShouldReturnAuthenticationData(string id, string emailAddress, string role, string accessToken)
    {
        // Arrange
        AuthenticationData expectedAuthenticationData = new(emailAddress, [role], accessToken);
        AuthenticationResponse validResponseBody = new("OK", new(id, emailAddress, DateTime.UtcNow.Ticks));
        HashSet<KeyValuePair<string, string>> headers = [new(_accessTokenHeaderKey, accessToken)];

        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(validResponseBody)
            .WithResponseHavingHeaders(headers)
            .WithGetRolesReturning([role])
            .Build();

        // Act
        var authenticationData = await authenticationClient.SignInAsync(_defaultUsername, _defaultPassword);

        // Assert
        authenticationData.ShouldBeEquivalentTo(expectedAuthenticationData);
    }

    [Theory]
    [AutoData]
    public async Task SignInAsync_WithMultipleAccessTokens_ShouldTakeFirstAccessToken(string id, string emailAddress, string role, string firstAccessToken, string secondAccessToken)
    {
        // Arrange
        AuthenticationData expectedAuthenticationData = new(emailAddress, [role], firstAccessToken);
        AuthenticationResponse validResponseBody = new("OK", new(id, emailAddress, DateTime.UtcNow.Ticks));
        HashSet<KeyValuePair<string, string>> headers =
        [
            new(_accessTokenHeaderKey, firstAccessToken),
            new(_accessTokenHeaderKey, secondAccessToken)
        ];

        var authenticationClient = new AuthenticationClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(validResponseBody)
            .WithResponseHavingHeaders(headers)
            .WithGetRolesReturning([role])
            .Build();

        // Act
        var authenticationData = await authenticationClient.SignInAsync(_defaultUsername, _defaultPassword);

        // Assert
        authenticationData.ShouldBeEquivalentTo(expectedAuthenticationData);
    }
}
