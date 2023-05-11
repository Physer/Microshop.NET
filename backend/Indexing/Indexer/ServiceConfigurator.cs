using Application.Options;
using Mapper;
using ProductsClient;
using Search;

namespace Indexer;

public static class ServiceConfigurator
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Options
        var productOptionsSection = context.Configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var productOptions = productOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(productOptionsSection);

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
    }
}
