using Microsoft.Extensions.Hosting;

namespace Indexer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        ServiceConfigurator.ConfigureServices(builder.Configuration, builder.Services);
        using var host = builder.Build();
        await host.StartAsync();
        var cancellationToken = args.Contains("ShouldStop") ? new CancellationToken(true) : default;
        await host.WaitForShutdownAsync(cancellationToken);
    }
}
