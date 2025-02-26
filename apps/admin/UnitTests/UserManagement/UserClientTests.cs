using Application.Exceptions;
using AutoFixture;
using AutoFixture.Xunit2;
using Shouldly;
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
        exception.ShouldBeOfType<MicroshopApiException>();
        exception.Message.ShouldBeEquivalentTo(expectedErrorMessage);
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
        exception.ShouldBeOfType<MicroshopApiException>();
        exception.Message.ShouldBeEquivalentTo(expectedErrorMessage);
    }

    [Fact]
    public async Task GetUsersAsync_WithValidData_ReturnsUsers()
    {
        // Arrange
        GetUsersResponse validGetUsersResponse = _fixture.Create<GetUsersResponse>();
        UserRolesResponse validUserRolesResponse = _fixture.Create<UserRolesResponse>();
        var userClient = new UserClientBuilder()
            .WithGetUserDataAsyncReturning(validGetUsersResponse)
            .WithGetUsersInAdminRoleAsyncReturning(validUserRolesResponse)
            .Build();

        // Act
        var users = userClient.GetUsersAsync();

        // Assert
        var userResponseEmails = validGetUsersResponse.Users.Select(usersResponse => usersResponse.User.Email);
        var userEmails = await users.Select(user => user.EmailAddress).ToListAsync();

        (await users.CountAsync()).ShouldBe(validGetUsersResponse.Users.Count());
        userEmails.ShouldBeEquivalentTo(userResponseEmails.ToList());
    }
}
