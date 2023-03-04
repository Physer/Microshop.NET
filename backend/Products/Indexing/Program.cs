using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using Indexing;
using Indexing.Options;
using Meilisearch;
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
        var indexingOptions = new IndexingOptions();
        var indexingOptionsConfigurationSection = context.Configuration.GetSection(IndexingOptions.ConfigurationEntry);
        indexingOptionsConfigurationSection.Bind(indexingOptions);

        // Products HTTP Client
        services.AddHttpClient<IProductsClient, ProductsHttpClient>(configuration => configuration.BaseAddress = new Uri(productsOptions?.BaseUrl ?? string.Empty));

        // Indexing
        services.AddSingleton<IIndexingService, IndexingService>();
        services.AddSingleton(_ => new MeilisearchClient(indexingOptions.BaseUrl, indexingOptions.ApiKey));
    })
    .Build();

host.Run();
