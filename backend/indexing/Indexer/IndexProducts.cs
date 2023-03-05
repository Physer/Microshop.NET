using Application.Interfaces.Indexing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Indexer;

public class IndexProducts
{
    private readonly ILogger _logger;
    private readonly IIndexingService _indexingService;

    public IndexProducts(ILoggerFactory loggerFactory,
        IIndexingService indexingService)
    {
        _logger = loggerFactory.CreateLogger<IndexProducts>();
        _indexingService = indexingService;
    }

    [Function(nameof(IndexProducts))]
    public async Task RunAsync([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {date}", DateTime.Now);

        _logger.LogInformation("Indexing all products...");
        var stopwatch = Stopwatch.StartNew();
        await _indexingService.IndexProductsAsync();
        stopwatch.Stop();
        _logger.LogInformation("Finished indexing in {time} ms", stopwatch.ElapsedMilliseconds);

        _logger.LogInformation("Next timer schedule at: {next}", myTimer?.ScheduleStatus?.Next);
    }
}
