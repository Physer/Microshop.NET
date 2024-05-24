using FluentAssertions;
using Microshop.MockHttpClient;
using NSubstitute;
using Tests.Builders;
using Tests.Utilities;

namespace Tests.Tests;

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
}
