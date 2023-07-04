namespace Indexer;

public class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(ServiceConfigurator.ConfigureServices)
            .Build();

        await host.StartAsync();
        var cancellationToken = args.Contains("ShouldStop") ? new CancellationToken(true) : default;
        await host.WaitForShutdownAsync(cancellationToken);
    }
}
