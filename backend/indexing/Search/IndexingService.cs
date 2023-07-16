using Application.Interfaces.Indexing;
using Application.Models;
using AutoMapper;
using Domain;

namespace Search;

public class IndexingService : IProductsIndexer, IPricesIndexer
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
}
