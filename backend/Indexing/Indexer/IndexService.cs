namespace Indexer;

public class IndexService : IHostedService, IDisposable
{
    private readonly ILogger<IndexService> _logger;

    private Timer? _timer;

    public IndexService(ILogger<IndexService> logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async _ => await Index(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async Task Index() => await Task.Run(() => _logger.LogInformation("Hit!"));
}