using Application;
using Indexing.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace Indexing;

public class IndexProducts
{
    private readonly ILogger _logger;
    private readonly ProductsOptions _productsOptions;
    private readonly IndexingOptions _indexingOptions;
    private readonly IIndexingService _indexingService;

    public IndexProducts(ILoggerFactory loggerFactory,
        IOptions<ProductsOptions> productOptions,
        IOptions<IndexingOptions> indexingOptions,
        IIndexingService indexingService)
    {
        _logger = loggerFactory.CreateLogger<IndexProducts>();
        _productsOptions = productOptions.Value;
        _indexingOptions = indexingOptions.Value;
        _indexingService = indexingService;
    }

    [Function(nameof(IndexProducts))]
    public async Task RunAsync([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {date}", DateTime.Now);
        _logger.LogInformation("Product options: {productOptions}", JsonSerializer.Serialize(_productsOptions));
        _logger.LogInformation("Indexing options: {indexingOptions}", JsonSerializer.Serialize(_indexingOptions));

        _logger.LogInformation("Indexing all products...");
        var stopwatch = Stopwatch.StartNew();
        await _indexingService.IndexProductsAsync();
        stopwatch.Stop();
        _logger.LogInformation("Finished indexing in {time} ms", stopwatch.ElapsedMilliseconds);

        _logger.LogInformation("Next timer schedule at: {next}", myTimer?.ScheduleStatus?.Next);
    }
}
