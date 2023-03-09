using Application.Interfaces.Indexing;
using Application.Options;
using Indexer;
using Mapper;
using Meilisearch;
using ProductsClient;
using Search;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Options
        var productOptionsSection = context.Configuration.GetSection(ProductsOptions.ConfigurationEntry);
        var productOptions = productOptionsSection.Get<ProductsOptions>();
        services.Configure<ProductsOptions>(productOptionsSection);

        var indexingOptionsSection = context.Configuration.GetSection(IndexingOptions.ConfigurationEntry);
        var indexingOptions = indexingOptionsSection.Get<IndexingOptions>();
        services.Configure<IndexingOptions>(indexingOptionsSection);

        // Products AMQP Client
        services.RegisterAmqpDependencies(productOptions);

        // Indexing
        services.AddSingleton<IIndexingService, IndexingService>();
        services.AddSingleton(_ => new MeilisearchClient(indexingOptions?.BaseUrl, indexingOptions?.ApiKey));

        // Automapper
        services.AddAutoMapper(typeof(ProductProfile));

        // Background service
        services.AddHostedService<IndexingWorker>();
    })
    .Build();

host.Run();
