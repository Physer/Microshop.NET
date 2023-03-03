using Application;
using Indexing;
using Indexing.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsClient;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Options
        var productsOptions = new ProductsOptions();
        var productsOptionsConfigurationSection = context.Configuration.GetSection(ProductsOptions.ConfigurationEntry);
        productsOptionsConfigurationSection.Bind(productsOptions);
        services.Configure<ProductsOptions>(productsOptionsConfigurationSection);
        services.Configure<IndexingOptions>(context.Configuration.GetSection(IndexingOptions.ConfigurationEntry));

        // Products HTTP Client
        services.AddHttpClient<IProductsClient, ProductsHttpClient>(configuration => configuration.BaseAddress = new Uri(productsOptions?.BaseUrl ?? string.Empty));

        // Indexing service
        services.AddSingleton<IIndexingService, IndexingService>();
    })
    .Build();

host.Run();
