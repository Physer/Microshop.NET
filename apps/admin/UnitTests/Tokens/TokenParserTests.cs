using Application.Authentication;
using Application.Exceptions;
using Authentication.Models;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Security.Claims;
using System.Text.Json;
using Xunit;

namespace UnitTests.Tokens;

public class TokenParserTests
{
    private readonly JsonSerializerOptions _defaultJsonSerializerOptions;

    public TokenParserTests() => _defaultJsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    [Fact]
    public void GetRoles_WithoutClaims_ThrowsAuthenticationException()
    {
        // Arrange
        var expectedErrorMessage = "Invalid role claim";
        Token emptyClaimsToken = new([]);
        var tokenParser = new TokenParserBuilder()
            .WithTokenHandlerReturning(emptyClaimsToken)
            .Build();

        // Act
        var exception = Record.Exception(() => tokenParser.GetRoles(string.Empty));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    }

    [Theory]
    [AutoData]
    public void GetRoles_WithInvalidClaims_ThrowsAuthenticationException(Dictionary<string, string> claimData)
    {
        // Arrange
        var expectedErrorMessage = "Invalid role claim";
        HashSet<Claim> claims = claimData.Select(data => new Claim(data.Key, data.Value)).ToHashSet();
        Token token = new(claims);
        var tokenParser = new TokenParserBuilder()
            .WithTokenHandlerReturning(token)
            .Build();

        // Act
        var exception = Record.Exception(() => tokenParser.GetRoles(string.Empty));

        // Assert
        exception.Should().BeOfType<AuthenticationException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    }

    [Theory]
    [AutoData]
    public void GetRoles_WithValidKey_WithInvalidValue_ThrowsJsonException(IEnumerable<object> claimValues)
    {
        // Arrange
        var validKey = "st-role";
        HashSet<Claim> claims = claimValues.Select(value => new Claim(validKey, JsonSerializer.Serialize(value, _defaultJsonSerializerOptions))).ToHashSet();
        Token token = new(claims);
        var tokenParser = new TokenParserBuilder()
            .WithTokenHandlerReturning(token)
            .Build();

        // Act
        var exception = Record.Exception(() => tokenParser.GetRoles(string.Empty));

        // Assert
        exception.Should().BeOfType<JsonException>();
    }

    [Fact]
    public void GetRoles_WithValidData_ReturnsRoles()
    {
        // Arrange
        var role = "User";
        MicroshopRoleClaim roleClaim = new()
        {
            AcquiredAt = DateTime.UtcNow.Ticks,
            Roles = [role]
        };
        Claim validRoleClaim = new("st-role", JsonSerializer.Serialize(roleClaim, _defaultJsonSerializerOptions));
        Token token = new([validRoleClaim]);
        var tokenParser = new TokenParserBuilder()
            .WithTokenHandlerReturning(token)
            .Build();

        // Act
        var getRolesResult = tokenParser.GetRoles(string.Empty);

        // Assert
        getRolesResult.Should().Contain(role);
        getRolesResult.Should().HaveCount(1);
    }
}
