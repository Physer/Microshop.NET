using Application.Models;
using AutoFixture.Xunit2;
using Domain;
using NSubstitute;
using UnitTests.Builders;
using Xunit;

namespace UnitTests;

public class IndexingServiceTests
{
    [Theory]
    [AutoData]
    public async Task IndexAsync_ShouldCallDependencies(IEnumerable<Product> products, IEnumerable<IndexableProduct> indexableProducts)
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder
            .WithMapperMappingToIndexableProducts(indexableProducts)
            .Build();

        // Act
        await indexingService.IndexAsync<Product, IndexableProduct>(products);

        // Assert
        indexingServiceBuilder._mapper.Received(1).Map<IEnumerable<IndexableProduct>>(products);
        await indexingServiceBuilder._index.ReceivedWithAnyArgs(1).AddOrUpdateDocumentsAsync(indexableProducts);
    }

    [Fact]
    public async Task IndexAsync_WithoutData_Returns()
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder
            .Build();

        // Act
        await indexingService.IndexAsync<Product, IndexableProduct>(default);

        // Assert
        indexingServiceBuilder._mapper.DidNotReceiveWithAnyArgs().Map<IEnumerable<IndexableProduct>>(Enumerable.Empty<object>());
        await indexingServiceBuilder._index.DidNotReceiveWithAnyArgs().AddOrUpdateDocumentsAsync(Enumerable.Empty<object>());
    }
}
