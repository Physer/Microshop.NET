using Application.Exceptions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UnitTests.Utilities;
using Xunit;

namespace UnitTests.DataManagement;

public class ApiClientTests
{
    private readonly HttpMethod _defaultHttpMethod;
    private readonly string _defaultRequestUrl;
    private readonly string _defaultAccessToken;

    public ApiClientTests()
    {
        _defaultHttpMethod = HttpMethod.Post;
        _defaultRequestUrl = "/valid-url";
        _defaultAccessToken = "access_token";
    }

    [Fact]
    public async Task MakeRequestAsync_WithoutValidAccessToken_ThrowsException()
    {
        // Arrange
        var apiClient = new ApiClientBuilder()
            .WithTokenRetrieverReturningToken(string.Empty)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => apiClient.MakeRequestAsync(_defaultHttpMethod, _defaultRequestUrl));

        // Assert
        exception.Should().NotBeNull();
    }

    [Theory]
    [ClassData(typeof(InvalidHttpResponseTestData))]
    public async Task MakeRequestAsync_WithoutSuccessStatusCode_ThrowsMicroshopApiException(HttpStatusCode statusCode)
    {
        // Arrange
        var apiClient = new ApiClientBuilder()
            .WithTokenRetrieverReturningToken(_defaultAccessToken)
            .WithResponseHavingStatusCode(statusCode)
            .Build();

        // Act
        var exception = await Record.ExceptionAsync(() => apiClient?.MakeRequestAsync(_defaultHttpMethod, _defaultRequestUrl));

        // Assert
        exception.Should().BeOfType<MicroshopApiException>();
    }

    [Fact]
    public async Task MakeRequestAsync_WithValidData_ReturnsResponse()
    {
        // Arrange
        MockHttpResponse expectedResponse = new("success");
        var statusCode = HttpStatusCode.OK;
        var apiClient = new ApiClientBuilder()
            .WithTokenRetrieverReturningToken(_defaultAccessToken)
            .WithResponseHavingStatusCode(statusCode)
            .WithResponseHavingContent(expectedResponse)
            .Build();

        // Act
        var response = await apiClient.MakeRequestAsync(_defaultHttpMethod, _defaultRequestUrl);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(statusCode);
        (await response.Content.ReadFromJsonAsync<MockHttpResponse>()).Should().BeEquivalentTo(expectedResponse);
    }
}
