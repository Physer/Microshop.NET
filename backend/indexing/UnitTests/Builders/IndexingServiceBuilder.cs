using Application.Interfaces.Indexing;
using Application.Models;
using AutoMapper;
using Domain;
using NSubstitute;
using Search;

namespace UnitTests.Builders;

internal class IndexingServiceBuilder
{
    internal readonly IMapper _mapper;
    internal readonly IIndex _index;

    public IndexingServiceBuilder()
    {
        _mapper = Substitute.For<IMapper>();
        _index = Substitute.For<IIndex>();
    }

    public IndexingServiceBuilder WithMapperMappingToIndexableProducts(IEnumerable<IndexableProduct> indexableProducts)
    {
        _mapper.Map(Arg.Any<IEnumerable<Product>>(), Arg.Any<IEnumerable<IndexableProduct>>()).Returns(indexableProducts);

        return this;
    }

    public IndexingService Build() => new(_mapper, _index);
}
