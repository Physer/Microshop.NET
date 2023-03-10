using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using Application.Models;
using AutoMapper;
using Meilisearch;

namespace Search;

public class IndexingService : IIndexingService
{
    private readonly IMapper _mapper;
    private readonly IProductsClient _productsClient;
    private readonly MeilisearchClient _meilisearchClient;

    public IndexingService(IMapper mapper,
        IProductsClient productsClient,
        MeilisearchClient meilisearchClient)
    {
        _mapper = mapper;
        _productsClient = productsClient;
        _meilisearchClient = meilisearchClient;
    }

    public async Task IndexProductsAsync(CancellationToken cancellationToken = default)
    {
        var indexableProducts = _mapper.Map<IEnumerable<IndexableProduct>>(await _productsClient.GetProductsAsync(cancellationToken));
        var index = _meilisearchClient.Index("products");
        await index.DeleteAllDocumentsAsync();
        await index.AddDocumentsAsync(indexableProducts);
    }
}
