using Application;

namespace Indexing;

public class IndexingService : IIndexingService
{
    private readonly IProductsClient _productsClient;

    public IndexingService(IProductsClient productsClient)
    {
        _productsClient = productsClient;
    }

    public async Task IndexProductsAsync()
    {
        _ = await _productsClient.GetProductsAsync();
    }
}
