using Application.Interfaces.Indexing;
using AutoMapper;

namespace Search;

internal class IndexingService : IIndexingService
{
    private readonly IMapper _mapper;
    private readonly IIndex _index;

    public IndexingService(IMapper mapper,
        IIndex index)
    {
        _mapper = mapper;
        _index = index;
    }

    public async Task IndexAsync<TInput, TIndexableModel>(IEnumerable<TInput>? dataToIndex)
    {
        if (dataToIndex is null)
            return;

        var indexableData = _mapper.Map<IEnumerable<TIndexableModel>>(dataToIndex);
        await _index.AddOrUpdateDocumentsAsync(indexableData);
    }
}
