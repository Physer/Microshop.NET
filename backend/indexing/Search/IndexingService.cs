using Application.Interfaces.Indexing;
using Application.Models;
using AutoMapper;
using Domain;

namespace Search;

public class IndexingService : IProductsIndexer, IPricesIndexer, IIndexingService
{
    private readonly IMapper _mapper;
    private readonly IIndexingClient _indexingClient;

    public IndexingService(IMapper mapper,
        IIndexingClient indexingClient)
    {
        _mapper = mapper;
        _indexingClient = indexingClient;
    }

    public async Task IndexProductsAsync(IEnumerable<Product> products)
    {
        var indexableProducts = _mapper.Map<IEnumerable<IndexableProduct>>(products);
        await _indexingClient.AddOrUpdateDocumentsAsync(indexableProducts);
    }

    public async Task IndexAsync<TInput, TIndexableModel>(IEnumerable<TInput>? dataToIndex)
    {
        if (dataToIndex is null)
            return;

        var indexableData = _mapper.Map<IEnumerable<TIndexableModel>>(dataToIndex);
        await _indexingClient.AddOrUpdateDocumentsAsync(indexableData);
    }
}
