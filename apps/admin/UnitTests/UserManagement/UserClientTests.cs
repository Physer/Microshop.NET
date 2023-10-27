using Application.Exceptions;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using UserManagement.Models;
using Xunit;

namespace UnitTests.UserManagement;

public class UserClientTests
{
    private readonly IFixture _fixture;

    public UserClientTests() => _fixture = new Fixture();

    [Theory]
    [AutoData]
    public async Task GetUsersAsync_WithInvalidUserData_ThrowsMicroshopApiException(object httpResponseObject)
    {
        // Arrange
        var expectedErrorMessage = "Unable to retrieve users data";
        var userClient = new UserClientBuilder()
            .WithGetUserDataAsyncReturning(httpResponseObject)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(async () => await userClient.GetUsersAsync().ToListAsync());

        // Assert
        exception.Should().BeOfType<MicroshopApiException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    }

    [Theory]
    [AutoData]
    public async Task GetUsersAsync_WithInvalidRoleData_ThrowsMicroshopApiException(object httpResponseObject)
    {
        // Arrange
        var expectedErrorMessage = "Unable to retrieve role data";
        GetUsersResponse validGetUsersResponse = _fixture.Create<GetUsersResponse>();
        var userClient = new UserClientBuilder()
            .WithGetUserDataAsyncReturning(validGetUsersResponse)
            .WithGetUsersInAdminRoleAsyncReturning(httpResponseObject)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(async () => await userClient.GetUsersAsync().ToListAsync());

        // Assert
        exception.Should().BeOfType<MicroshopApiException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    }
}
