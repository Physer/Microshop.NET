using Application.Interfaces.Indexing;
using AutoMapper;

namespace Search;

public class IndexingService : IIndexingService
{
    private readonly IMapper _mapper;
    private readonly IIndexingClient _indexingClient;

    public IndexingService(IMapper mapper,
        IIndexingClient indexingClient)
    {
        _mapper = mapper;
        _indexingClient = indexingClient;
    }

    public async Task IndexAsync<TInput, TIndexableModel>(IEnumerable<TInput>? dataToIndex)
    {
        if (dataToIndex is null)
            return;

        var indexableData = _mapper.Map<IEnumerable<TIndexableModel>>(dataToIndex);
        await _indexingClient.AddOrUpdateDocumentsAsync(indexableData);
    }
}
