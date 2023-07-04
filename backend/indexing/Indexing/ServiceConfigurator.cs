using Application.Options;
using Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsClient;
using Search;

namespace Indexer;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var productOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var productOptions = productOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(productOptionsSection);

        var indexingOptionsSection = configuration.GetSection(IndexingOptions.ConfigurationEntry);
        var indexingOptions = indexingOptionsSection.Get<IndexingOptions>();
        services.Configure<IndexingOptions>(indexingOptionsSection);

        // Products AMQP Client
        services.RegisterAmqpDependencies(productOptions);

        // Indexing
        services.RegisterSearchDependencies(indexingOptions);

        // Automapper
        services.RegisterMapperDependencies();
    }
}
