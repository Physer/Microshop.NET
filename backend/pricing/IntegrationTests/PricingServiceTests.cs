using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Messaging.Messages;
using Xunit;

namespace IntegrationTests;

public class PricingServiceTests : IClassFixture<PricingServiceTestsFixture>
{
    private readonly PricingServiceTestsFixture _fixture;

    public PricingServiceTests(PricingServiceTestsFixture fixture) => _fixture = fixture;

    [Theory]
    [AutoData]
    public async Task ReceivesMessage_ProductsGenerated_PublishesPricesGeneratedMessage(IEnumerable<Product> products)
    {
        // Arrange
        var testHarness = _fixture.TestHarness!;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<ProductsGenerated>(new(products));
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<ProductsGenerated>()).Should().BeTrue();
        (await testHarness.Published.Any<PricesGenerated>()).Should().BeTrue();
    }
}
