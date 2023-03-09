using Application.Options;
using Indexer;
using Mapper;
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
        services.RegisterSearchDependencies(indexingOptions);

        // Automapper
        services.RegisterMapperDependencies();

        // Background service
        services.AddHostedService<IndexingWorker>();
    })
    .Build();

host.Run();
