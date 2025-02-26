using Application.Exceptions;
using Microshop.MockHttpClient;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
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

    [Theory]
    [ClassData(typeof(InvalidHttpResponseClassData))]
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
        exception.ShouldBeOfType<MicroshopApiException>();
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
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(statusCode);
        (await response.Content.ReadFromJsonAsync<MockHttpResponse>()).ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GenerateProductsAsync_CallsDataEndpoint_WithHttpPost()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Post;
        var expectedRequestUrl = "/data";
        var apiClient = new ApiClientBuilder()
            .WithMakeRequestAsyncUsing(expectedHttpMethod, expectedRequestUrl)
            .Build();

        // Act
        var response = await apiClient.GenerateProductsAsync();

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.Method.ShouldBeEquivalentTo(expectedHttpMethod);
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo(expectedRequestUrl);
    }

    [Fact]
    public async Task ClearDataAsync_CallsDataEndpoint_WithHttpDelete()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Delete;
        var expectedRequestUrl = "/data";
        var apiClient = new ApiClientBuilder()
            .WithMakeRequestAsyncUsing(expectedHttpMethod, expectedRequestUrl)
            .Build();

        // Act
        var response = await apiClient.ClearDataAsync();

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.Method.ShouldBeEquivalentTo(expectedHttpMethod);
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo(expectedRequestUrl);

    }
}
