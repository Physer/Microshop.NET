using Indexing.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Options
        services.Configure<ProductsOptions>(context.Configuration.GetSection(ProductsOptions.ConfigurationEntry));
        services.Configure<IndexingOptions>(context.Configuration.GetSection(IndexingOptions.ConfigurationEntry));
    })
    .Build();

host.Run();
