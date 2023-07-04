using Application.Models;
using AutoFixture.Xunit2;
using Domain;
using NSubstitute;
using Tests.Builders;
using Xunit;

namespace Tests;

public class IndexingServiceTests
{

    [Theory]
    [AutoData]
    public async Task IndexProductsAsync_ShouldCallDependencies(IEnumerable<Product> products, IEnumerable<IndexableProduct> indexableProducts)
    {
        // Arrange
        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder
            .WithMapperMappingToIndexableProducts(indexableProducts)
            .Build();

        // Act
        await indexingService.IndexProductsAsync(products);

        // Assert
        indexingServiceBuilder._mapperMock.Received(1).Map<IEnumerable<IndexableProduct>>(products);
        await indexingServiceBuilder._indexingClientMock.Received(1).DeleteAllDocumentsAsync(indexingServiceBuilder._indexMock);
        await indexingServiceBuilder._indexingClientMock.ReceivedWithAnyArgs(1).AddDocumentsAsync(indexingServiceBuilder._indexMock, indexableProducts);
    }
}
