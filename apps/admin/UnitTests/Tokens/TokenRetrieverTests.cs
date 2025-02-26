using Application;
using AutoFixture.Xunit2;
using Shouldly;
using Xunit;

namespace UnitTests.Tokens;

public class TokenRetrieverTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetAccessTokenFromCookie_WithInvalidCookieName_ShouldThrowException(string cookiename)
    {
        // Arrange
        var tokenRetriever = new TokenRetrieverBuilder()
            .WithAuthorizationCookieData(cookiename, string.Empty)
            .Build();

        // Act
        var exception = Record.Exception(tokenRetriever.GetAccessTokenFromCookie);

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UnauthorizedAccessException>();
    }

    [Theory]
    [AutoData]
    public void GetAccessTokenFromCookie_WithUnknownCookieName_ShouldThrowException(string cookiename)
    {
        // Arrange
        var tokenRetriever = new TokenRetrieverBuilder()
            .WithAuthorizationCookieData(cookiename, string.Empty)
            .Build();

        // Act
        var exception = Record.Exception(tokenRetriever.GetAccessTokenFromCookie);

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UnauthorizedAccessException>();
    }

    [Theory]
    [AutoData]
    public void GetAccessTokenFromCookie_WithAuthorizationCookie_ShouldReturnAccessToken(string accessToken)
    {
        // Arrange
        var cookieName = Globals.Cookies.AuthorizationTokenCookieName;
        var tokenRetriever = new TokenRetrieverBuilder()
            .WithAuthorizationCookieData(cookieName, accessToken)
            .Build();

        // Act
        var result = tokenRetriever.GetAccessTokenFromCookie();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(accessToken);
    }
}
