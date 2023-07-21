using Application.Interfaces.Indexing;
using AutoMapper;
using Mapper;
using NSubstitute;
using Search;

namespace UnitTests.Indexing;

internal class IndexingServiceBuilder
{
    internal readonly IIndex _index;
    internal readonly IMapper _mapper;

    public IndexingServiceBuilder()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(IndexingProfile))).CreateMapper();
        _index = Substitute.For<IIndex>();
    }

    public IndexingService Build() => new(_mapper, _index);
}
