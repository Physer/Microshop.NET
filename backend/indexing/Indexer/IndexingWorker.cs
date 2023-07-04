using Application.Interfaces.Indexing;
using Application.Options;
using Domain;
using Microsoft.Extensions.Options;

namespace Indexer;

public class IndexingWorker : IHostedService, IDisposable
{
    private readonly ILogger<IndexingWorker> _logger;
    private readonly IIndexingService _indexingService;
    private readonly IndexingOptions _indexingOptions;

    private Timer? _timer;

    public IndexingWorker(ILogger<IndexingWorker> logger, 
        IIndexingService indexingService,
        IOptions<IndexingOptions> indexingOptions)
    {
        _logger = logger;
        _indexingService = indexingService;
        _indexingOptions = indexingOptions.Value;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var interval = _indexingOptions.IndexingIntervalInSeconds ?? 3600;
        _logger.LogInformation("Triggering service every {interval} seconds", interval);
        _timer = new Timer(async _ => await IndexAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(interval));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async Task IndexAsync()
    {
        _logger.LogInformation("Starting indexing");
        await _indexingService.IndexProductsAsync(Enumerable.Empty<Product>());
        _logger.LogInformation("Done indexing");
    }
}