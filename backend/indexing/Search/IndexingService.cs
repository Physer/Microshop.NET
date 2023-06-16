using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using Application.Models;
using AutoMapper;

namespace Search;

public class IndexingService : IIndexingService
{
    private readonly IMapper _mapper;
    private readonly IProductsClient _productsClient;
    private readonly IIndexingClient _indexingClient;
    private readonly IMicroshopIndex _index;

    public IndexingService(IMapper mapper,
        IProductsClient productsClient,
        IIndexingClient indexingClient,
        IMicroshopIndex index)
    {
        _mapper = mapper;
        _productsClient = productsClient;
        _indexingClient = indexingClient;
        _index = index;
    }

    public async Task IndexProductsAsync(CancellationToken cancellationToken = default)
    {
        var indexableProducts = _mapper.Map<IEnumerable<IndexableProduct>>(await _productsClient.GetProductsAsync(cancellationToken));
        await _indexingClient.DeleteAllDocumentsAsync(_index);
        await _indexingClient.AddDocumentsAsync(_index, indexableProducts);
    }
}
