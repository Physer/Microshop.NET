using Application.Interfaces.Indexing;
using Application.Options;
using Mapper;
using Meilisearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsClient;
using Search;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Options
        ProductsOptions productsOptions = new();
        var productsOptionsConfigurationSection = context.Configuration.GetSection(ProductsOptions.ConfigurationEntry);
        productsOptionsConfigurationSection.Bind(productsOptions);
        IndexingOptions indexingOptions = new();
        var indexingOptionsConfigurationSection = context.Configuration.GetSection(IndexingOptions.ConfigurationEntry);
        indexingOptionsConfigurationSection.Bind(indexingOptions);

        // Products AMQP Client
        services.RegisterAmqpDependencies(productsOptions);

        // Indexing
        services.AddSingleton<IIndexingService, IndexingService>();
        services.AddSingleton(_ => new MeilisearchClient(indexingOptions.BaseUrl, indexingOptions.ApiKey));

        // Automapper
        services.AddAutoMapper(typeof(ProductProfile));
    })
    .Build();

host.Run();
