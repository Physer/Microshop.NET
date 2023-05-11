using Indexer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace IntegrationTests;

public class IndexingWorkerTests
{
    [Fact]
    public async Task TestIndexingWorker()
    {
        // Arrange
        var hostBuilder = new HostBuilder().ConfigureServices(services =>
        {
            services.AddHostedService<IndexingWorker>();
        });
        var host = hostBuilder.Build();
        await host.StartAsync();
        var indexingWorker = host.Services.GetRequiredService<IndexingWorker>();

        // Act
        var cancellationToken = new CancellationToken();
        await indexingWorker.StartAsync(cancellationToken);
        Thread.Sleep(5000);
        await indexingWorker.StopAsync(cancellationToken);

        // Assert
        // TODO: Check Indexed documents
    }
}
