using Microsoft.Extensions.Hosting;

namespace ProductsGenerator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args).ConfigureServices(ServiceConfigurator.ConfigureServices);
        using var host = builder.Build();
        await host.StartAsync();
        await Executor.GenerateAsync(host);
        await host.StopAsync();
    }
}