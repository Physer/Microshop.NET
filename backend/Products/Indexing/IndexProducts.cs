using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Indexing;

public class IndexProducts
{
    private readonly ILogger _logger;

    public IndexProducts(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<IndexProducts>();
    }

    [Function(nameof(IndexProducts))]
    public void Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {date}", DateTime.Now);
        _logger.LogInformation("Next timer schedule at: {next}", myTimer?.ScheduleStatus?.Next);
    }
}
