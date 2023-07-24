using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Messaging.Messages;
using Xunit;

namespace IntegrationTests;

public class IndexingTests : IClassFixture<IndexingTestsFixture>
{
    private readonly IndexingTestsFixture _fixture;

    public IndexingTests(IndexingTestsFixture fixture) => _fixture = fixture;

    [Theory]
    [AutoData]
    public async void ReceivesMessage_ProductsGenerated_IndexesProducts(IEnumerable<Product> products)
    {
        // Arrange
        var testHarness = _fixture.TestHarness!;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<ProductsGenerated>(new(products));
        await testHarness.Stop();

        // Assert
        var containerLogs = await _fixture.MeilisearchContainer!.GetLogsAsync();
        containerLogs.Should().NotBeNull();
        containerLogs.Stderr.Should().Contain($"indexed_documents: {products.Count()}");
    }
}
