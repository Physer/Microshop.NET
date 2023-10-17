using Application;
using FluentAssertions;
using Xunit;

namespace UnitTests.Tokens;

public class TokenRetrieverTests
{
    [Theory]
    [InlineData("Auth")]
    [InlineData("invalid")]
    [InlineData("1")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GetAccessTokenFromCookie_WithWrongCookieName_ShouldThrowException(string cookiename)
    {
        // Arrange
        var tokenRetriever = new TokenRetrieverBuilder()
            .WithAuthorizationCookieData(cookiename, string.Empty)
            .Build();

        // Act
        var exception = Record.Exception(() => tokenRetriever.GetAccessTokenFromCookie());

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public void GetAccessTokenFromCookie_WithAuthorizationCookie_ShouldReturnAccessToken()
    {
        // Arrange
        var cookieName = Globals.Cookies.AuthorizationTokenCookieName;
        var accessToken = "access_token";
        var tokenRetriever = new TokenRetrieverBuilder()
            .WithAuthorizationCookieData(cookieName, accessToken)
            .Build();

        // Act
        var result = tokenRetriever.GetAccessTokenFromCookie();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(accessToken);
    }
}
