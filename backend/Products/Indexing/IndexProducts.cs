using Indexing.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Indexing;

public class IndexProducts
{
    private readonly ILogger _logger;
    private readonly ProductsOptions _productsOptions;
    private readonly IndexingOptions _indexingOptions;

    public IndexProducts(ILoggerFactory loggerFactory,
        IOptions<ProductsOptions> productOptions,
        IOptions<IndexingOptions> indexingOptions)
    {
        _logger = loggerFactory.CreateLogger<IndexProducts>();
        _productsOptions = productOptions.Value;
        _indexingOptions = indexingOptions.Value;
    }

    [Function(nameof(IndexProducts))]
    public void Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {date}", DateTime.Now);
        _logger.LogInformation("Product options: {productOptions}", JsonSerializer.Serialize(_productsOptions));
        _logger.LogInformation("Indexing options: {indexingOptions}", JsonSerializer.Serialize(_indexingOptions));
        _logger.LogInformation("Next timer schedule at: {next}", myTimer?.ScheduleStatus?.Next);
    }
}
