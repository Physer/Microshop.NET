# Microshop.NET - MockHttpClient library

# Introduction

The MockHttpClient library for Microshop.NET allows applications to easily and quickly set-up mocked HTTP requests and responses for testing purposes.

# Features

- Allows mocking of [Typed HTTP Clients](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#how-to-use-typed-clients-with-ihttpclientfactory)
- Closely integrated with [XUnit](https://xunit.net/)
- Fully covered by automated tests
- Exposes fields and methods through inheritance
- Allows creation of predefined fake HTTP messages for easy scaffolding
- Ties in to and uses the [Builder pattern](https://refactoring.guru/design-patterns/builder)
- Ties in to the [Arrange, Act, Assert testing pattern](https://learn.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2022#write-your-tests)

# How to use

Depending on your use case, you can use a predefined mocked HTTP client or create an elaborate set-up for mocking HTTP responses with your own messages.

Start by installing the MockHttpClient NuGet package in your test project: `dotnet add package Microshop.MockHttpClient`.

## Basic usage

The library allows you to specify the following entities:

- The HTTP response code
- The response object
- The response headers

Whilst it's possible to specify these values, there are defaults available when it's not necessary to customize these.

The default values are:

- A 200 OK HTTP response code
- An empty response object
- Empty response headers

For this simple use-case, follow these instructions:

1. Create a class that inherits from `HttpClientBuilder<T>` where `T` is your builder class, if any, or your test class
2. Create an HTTP client for use in your test files by calling the method `BuildHttpClient()`. This method returns an `HttpClient`
3. Prepare your System-Under-Test by injecting the `HttpClient` object returned by the `BuildHttpClient()` method

Here's some example code. For brevity, the dependency injection configuration has been omitted. Assume all necessary things are done to create a typed HTTP client.

In this example we have a service that retrieves comments from an external source. We want to write tests for this service but not call the external source. In order to do so we will use the `MockHttpClient` library.

The service that calls the external source looks like this:

```csharp
public class ExternalDataService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<HttpResponseMessage> GetCommentsAsync() => await _httpClient.GetAsync("/comments");
}
```

We can write a unit test for testing `GetCommentsAsync` like so:

```csharp
public class ExternalDataServiceTests : HttpClientBuilder<ExternalDataServiceTests>
{
    [Fact]
    public async Task GetCommentsAsync_Returns200Ok()
    {
        // Arrange
        var httpClient = BuildHttpClient();

        ExternalDataService externalDataService = new(httpClient);

        // Act
        var result = await externalDataService.GetCommentsAsync();

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

Note that in a real-life situation your service will probably do more than returning the HTTP response.

In case you'd like to have more control over the HTTP content returned, take a look at the [Advanced usage](#advanced-usage).

## Advanced usage

WIP
