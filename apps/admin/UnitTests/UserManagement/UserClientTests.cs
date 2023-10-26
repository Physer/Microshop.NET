using Application.Exceptions;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Net;
using Xunit;

namespace UnitTests.UserManagement;

public class UserClientTests
{
    [Theory]
    [AutoData]
    public async Task GetUsersAsync_WithInvalidUserData_ThrowsMicroshopApiException(object httpResponseObject)
    {
        // Arrange
        var expectedErrorMessage = "Unable to retrieve users data";
        var userClient = new UserClientBuilder()
            .WithResponseHavingStatusCode(HttpStatusCode.OK)
            .WithResponseHavingContent(httpResponseObject)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(async () => await userClient.GetUsersAsync().ToListAsync());

        // Assert
        exception.Should().BeOfType<MicroshopApiException>();
        exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    }

    //[Theory]
    //[AutoData]
    //public async Task GetUsersAsync_WithInvalidRoleData_ThrowsMicroshopApiException(object httpResponseObject)
    //{
    //    // Arrange
    //    var expectedErrorMessage = "Unable to retrieve role data";
    //    var userClient = new UserClientBuilder()
    //        .WithResponseHavingStatusCode(HttpStatusCode.OK)
    //        .WithResponseHavingContent(httpResponseObject)
    //        .Build();

    //    // Act
    //    var exception = await Record.ExceptionAsync(async () => await userClient.GetUsersAsync().ToListAsync());

    //    // Assert
    //    exception.Should().BeOfType<MicroshopApiException>();
    //    exception.Message.Should().BeEquivalentTo(expectedErrorMessage);
    //}
}
