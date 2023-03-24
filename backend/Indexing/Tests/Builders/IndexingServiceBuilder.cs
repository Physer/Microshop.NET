using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using Application.Models;
using AutoMapper;
using Domain;
using NSubstitute;
using Search;

namespace Tests.Builders;

internal class IndexingServiceBuilder
{
    internal readonly IMapper _mapperMock;
    internal readonly IProductsClient _productsClientMock;
    internal readonly IIndexingClient _indexingClientMock;
    internal readonly IMicroshopIndex _indexMock;

    public IndexingServiceBuilder()
    {
        _mapperMock = Substitute.For<IMapper>();
        _productsClientMock = Substitute.For<IProductsClient>();
        _indexingClientMock = Substitute.For<IIndexingClient>();
        _indexMock = Substitute.For<IMicroshopIndex>();
    }

    public IndexingServiceBuilder WithProductsClientReturningProducts(IEnumerable<Product> products)
    {
        _productsClientMock.GetProductsAsync(Arg.Any<CancellationToken>()).Returns(products);

        return this;
    }

    public IndexingServiceBuilder WithMapperMappingToIndexableProducts(IEnumerable<IndexableProduct> indexableProducts)
    {
        _mapperMock.Map(Arg.Any<IEnumerable<IndexableProduct>>(), Arg.Any<IEnumerable<Product>>()).Returns(indexableProducts);

        return this;
    }

    public IndexingService Build() => new(_mapperMock, _productsClientMock, _indexingClientMock, _indexMock);
}
