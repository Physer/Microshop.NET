using Application.Interfaces.Indexing;
using Application.Models;
using AutoMapper;
using Domain;
using NSubstitute;
using Search;

namespace Tests.Builders;

internal class IndexingServiceBuilder
{
    internal readonly IMapper _mapperMock;
    internal readonly IIndexingClient _indexingClientMock;
    internal readonly IMicroshopIndex _indexMock;

    public IndexingServiceBuilder()
    {
        _mapperMock = Substitute.For<IMapper>();
        _indexingClientMock = Substitute.For<IIndexingClient>();
        _indexMock = Substitute.For<IMicroshopIndex>();
    }

    public IndexingServiceBuilder WithMapperMappingToIndexableProducts(IEnumerable<IndexableProduct> indexableProducts)
    {
        _mapperMock.Map(Arg.Any<IEnumerable<IndexableProduct>>(), Arg.Any<IEnumerable<Product>>()).Returns(indexableProducts);

        return this;
    }

    public IndexingService Build() => new(_mapperMock, _indexingClientMock, _indexMock);
}
