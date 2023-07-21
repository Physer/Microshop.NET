using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Messaging.Messages;
using Xunit;

namespace IntegrationTests;

public class ConsumerTests : IClassFixture<ConsumerTestsFixture>
{
    private readonly ConsumerTestsFixture _fixture;

    public ConsumerTests(ConsumerTestsFixture fixture) => _fixture = fixture;

    [Theory]
    [AutoData]
    public async void ProductsGeneratedConsumer_ConsumesMessage(IEnumerable<Product> products)
    {
        // Arranges
        var testHarness = _fixture.TestHarness;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<ProductsGenerated>(new(products));
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<ProductsGenerated>()).Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async void PricesGeneratedConsumer_ConsumesMessage(IEnumerable<Price> prices)
    {
        // Arranges
        var testHarness = _fixture.TestHarness;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<PricesGenerated>(new(prices));
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<PricesGenerated>()).Should().BeTrue();
    }
}
