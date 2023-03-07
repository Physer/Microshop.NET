using Application.Interfaces.Indexing;
using Microsoft.Azure.Functions.Worker;

namespace Indexer;

public class IndexProducts
{
    private readonly IIndexingService _indexingService;

    public IndexProducts(IIndexingService indexingService) => _indexingService = indexingService;

    [Function(nameof(IndexProducts))]
    public async Task RunAsync([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo timer) => await _indexingService.IndexProductsAsync();
}
