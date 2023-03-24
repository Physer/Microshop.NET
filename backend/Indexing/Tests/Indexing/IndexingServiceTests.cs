using Application.Models;
using AutoFixture;
using Domain;
using NSubstitute;
using Xunit;

namespace Tests.Indexing;

public class IndexingServiceTests
{
    private readonly IFixture _fixture;

    public IndexingServiceTests() => _fixture = new Fixture();

    [Fact]
    public async Task IndexProductsAsync_ShouldCallDependencies()
    {
        // Arrange
        var products = _fixture.Create<IEnumerable<Product>>();
        var indexableProducts = _fixture.Create<IEnumerable<IndexableProduct>>();

        var indexingServiceBuilder = new IndexingServiceBuilder();
        var indexingService = indexingServiceBuilder
            .WithProductsClientReturningProducts(products)
            .WithMapperMappingToIndexableProducts(indexableProducts)
            .Build();

        // Act
        await indexingService.IndexProductsAsync();

        // Assert
        indexingServiceBuilder._mapperMock.Received(1).Map<IEnumerable<IndexableProduct>>(products);
        await indexingServiceBuilder._indexingClientMock.Received(1).DeleteAllDocumentsAsync(indexingServiceBuilder._indexMock);
        await indexingServiceBuilder._indexingClientMock.ReceivedWithAnyArgs(1).AddDocumentsAsync(indexingServiceBuilder._indexMock, indexableProducts);
    }
}
