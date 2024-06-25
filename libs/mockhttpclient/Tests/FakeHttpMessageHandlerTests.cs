using FluentAssertions;
using Microshop.MockHttpClient;
using NSubstitute;
using Tests.Builders;
using Tests.Data;
using Tests.Utilities;

namespace Tests;

public class FakeHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_WithoutHttpMessages_ThrowsFakeHttpMessageException()
    {
        // Arrange
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().Build();

        // Act
        var exception = await Record.ExceptionAsync(() => fakeHttpMessageHandler.TestableSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<FakeHttpMessageException>();
    }

    [Fact]
    public async Task SendAsync_WithhMultipleMessagesAndNoMatchingUri_ThrowsFakeHttpMessageException()
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage1 = new();
        FakeHttpMessage fakeHttpMessage2 = new();
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage1, fakeHttpMessage2]).Build();

        // Act
        var exception = await Record.ExceptionAsync(() => fakeHttpMessageHandler.TestableSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<FakeHttpMessageException>();
    }

    [Fact]
    public async Task SendAsync_WithMultipleMessagesAndNoRequestUri_ThrowsFakeHttpMessageException()
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage1 = new();
        FakeHttpMessage fakeHttpMessage2 = new();
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage1, fakeHttpMessage2]).Build();
        HttpRequestMessage httpRequestMessage = new();

        // Act
        var exception = await Record.ExceptionAsync(() => fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<FakeHttpMessageException>();
    }

    [Fact]
    public async Task SendAsync_WithMultipleMessagesAndRequestUri_AndNoHttpRequestUrl_ThrowsFakeHttpMessageException()
    {
        // Arrange
        var requestUri1 = "/expected";
        var requestUri2 = "/unexpected";
        FakeHttpMessage fakeHttpMessage1 = new() { RequestUrl = requestUri1 };
        FakeHttpMessage fakeHttpMessage2 = new() { RequestUrl = requestUri2 };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage1, fakeHttpMessage2]).Build();
        HttpRequestMessage httpRequestMessage = new();

        // Act
        var exception = await Record.ExceptionAsync(() => fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<FakeHttpMessageException>();
    }

    [Theory]
    [ClassData(typeof(InvalidFakeHttpMessageHeaderValues))]
    public async Task SendAsync_WithInvalidHeaders_ThrowsException(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage = new() { Headers = headers };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage]).Build();

        // Act
        var exception = await Record.ExceptionAsync(() => fakeHttpMessageHandler.TestableSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public async Task SendAsync_WithOneMessageAndNoMatchingUri_ReturnsHttpResponseMessage()
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage = new();
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage]).Build();
        HttpRequestMessage httpRequestMessage = new();
        HttpResponseMessage expectedResponseMessage = new(fakeHttpMessage.StatusCode)
        {
            Content = new StringContent(string.Empty),
            RequestMessage = httpRequestMessage
        };

        // Act
        HttpResponseMessage actualResponseMessage = await fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>());

        // Assert
        actualResponseMessage.Should().NotBeNull();
        actualResponseMessage.Should().BeEquivalentTo(expectedResponseMessage);
    }

    [Fact]
    public async Task SendAsync_WithOneMessageAndUri_ReturnsHttpResponseMessage()
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage = new() { RequestUrl = "https://request.url.local" };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage]).Build();
        HttpRequestMessage httpRequestMessage = new();
        HttpResponseMessage expectedResponseMessage = new(fakeHttpMessage.StatusCode)
        {
            Content = new StringContent(string.Empty),
            RequestMessage = httpRequestMessage
        };

        // Act
        HttpResponseMessage actualResponseMessage = await fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>());

        // Assert
        actualResponseMessage.Should().NotBeNull();
        actualResponseMessage.Should().BeEquivalentTo(expectedResponseMessage);
    }

    [Fact]
    public async Task SendAsync_WithMultipleMessagesAndAbsoluteUris_ReturnsHttpResponseMessage()
    {
        // Arrange
        var expectedRequestUri = "https://microshop.tests/expected";
        var otherRequestUri = "https://microshop.tests/unexpected";
        FakeHttpMessage expectedFakeHttpMessage = new() { RequestUrl = expectedRequestUri };
        FakeHttpMessage otherFakeHttpMessage = new() { RequestUrl = otherRequestUri };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([expectedFakeHttpMessage, otherFakeHttpMessage]).Build();
        HttpRequestMessage httpRequestMessage = new() { RequestUri = new(expectedRequestUri, UriKind.Absolute) };
        HttpResponseMessage expectedResponseMessage = new(expectedFakeHttpMessage.StatusCode)
        {
            Content = new StringContent(string.Empty),
            RequestMessage = httpRequestMessage
        };

        // Act
        HttpResponseMessage actualResponseMessage = await fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>());

        // Assert
        actualResponseMessage.Should().NotBeNull();
        actualResponseMessage.Should().BeEquivalentTo(expectedResponseMessage);
    }

    [Fact]
    public async Task SendAsync_WithMultipleMessagesAndRelativeUris_ReturnsHttpResponseMessage()
    {
        // Arrange
        var expectedRelativeRequestUri = "/expected";
        var otherRelativeRequestUri = "/unexpected";
        FakeHttpMessage expectedFakeHttpMessage = new() { RequestUrl = expectedRelativeRequestUri };
        FakeHttpMessage otherFakeHttpMessage = new() { RequestUrl = otherRelativeRequestUri };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([expectedFakeHttpMessage, otherFakeHttpMessage]).Build();
        HttpRequestMessage httpRequestMessage = new() { RequestUri = new(expectedRelativeRequestUri, UriKind.Relative) };
        HttpResponseMessage expectedResponseMessage = new(expectedFakeHttpMessage.StatusCode)
        {
            Content = new StringContent(string.Empty),
            RequestMessage = httpRequestMessage
        };

        // Act
        HttpResponseMessage actualResponseMessage = await fakeHttpMessageHandler.TestableSendAsync(httpRequestMessage, Arg.Any<CancellationToken>());

        // Assert
        actualResponseMessage.Should().NotBeNull();
        actualResponseMessage.Should().BeEquivalentTo(expectedResponseMessage);
    }

    [Fact]
    public async Task SendAsync_WithoutHeaders_DoesNotAddHeaders()
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage = new();
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage]).Build();

        // Act
        HttpResponseMessage responseMessage = await fakeHttpMessageHandler.TestableSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());

        // Assert
        responseMessage.Should().NotBeNull();
        responseMessage.Headers.Should().BeEmpty();
    }

    [Theory]
    [ClassData(typeof(ValidFakeHttpMessageHeaderValues))]
    public async Task SendAsync_WithHeaders_AddsHeadersToResponseMessage(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        // Arrange
        FakeHttpMessage fakeHttpMessage = new() { Headers = headers };
        TestableFakeHttpMessageHandler fakeHttpMessageHandler = new FakeHttpMessageHandlerBuilder().WithMessages([fakeHttpMessage]).Build();

        // Act
        HttpResponseMessage responseMessage = await fakeHttpMessageHandler.TestableSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());

        // Assert
        var enumeratedHeaders = responseMessage.Headers.Select(header => new { header.Key, Value = header.Value.ToList() });
        var expectedHeaders = headers.Where(header => header.Value.Any(value => !string.IsNullOrWhiteSpace(value)));
        responseMessage.Should().NotBeNull();
        enumeratedHeaders.Should().BeEquivalentTo(expectedHeaders);
    }
}
