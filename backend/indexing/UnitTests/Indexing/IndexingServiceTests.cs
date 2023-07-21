using Application.Models;
using AutoFixture.Xunit2;
using Domain;
using NSubstitute;
using Xunit;

namespace UnitTests.Indexing;

public class IndexingServiceTests
{
    [Theory]
    [AutoData]
    public async Task IndexAsync_ShouldCallDependencies(IEnumerable<Product> products)
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder.Build();

        // Act
        await indexingService.IndexAsync<Product, IndexableProduct>(products);

        // Assert
        await indexingServiceBuilder._index.Received(1).AddOrUpdateDocumentsAsync(Arg.Any<IEnumerable<IndexableProduct>>());
    }

    [Fact]
    public async Task IndexAsync_WithoutData_Returns()
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder.Build();

        // Act
        await indexingService.IndexAsync<Product, IndexableProduct>(default);

        // Assert
        await indexingServiceBuilder._index.DidNotReceive().AddOrUpdateDocumentsAsync(Arg.Any<IEnumerable<IndexableProduct>>());
    }
}
