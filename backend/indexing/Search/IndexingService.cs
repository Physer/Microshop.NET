using Application.Interfaces.Indexing;
using Application.Models;
using AutoMapper;
using Domain;

namespace Search;

public class IndexingService : IProductsIndexer, IPricesIndexer
{
    private readonly IMapper _mapper;
    private readonly IIndexingClient _indexingClient;
    private readonly IMicroshopIndex _index;

    public IndexingService(IMapper mapper,
        IIndexingClient indexingClient,
        IMicroshopIndex index)
    {
        _mapper = mapper;
        _indexingClient = indexingClient;
        _index = index;
    }

    public async Task IndexProductsAsync(IEnumerable<Product> products)
    {
        var indexableProducts = _mapper.Map<IEnumerable<IndexableProduct>>(products);
        await _indexingClient.DeleteAllDocumentsAsync(_index);
        await _indexingClient.AddDocumentsAsync(_index, indexableProducts);
    }

    public async Task IndexPricesAsync(IEnumerable<Price> prices)
    {

    }
}
