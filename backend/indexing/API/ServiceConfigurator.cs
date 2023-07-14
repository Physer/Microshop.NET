using Application.Options;
using Mapper;
using Messaging;
using Search;

namespace API;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var servicebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servicebusOptionsSection);

        var indexingOptionsSection = configuration.GetSection(IndexingOptions.ConfigurationEntry);
        var indexingOptions = indexingOptionsSection.Get<IndexingOptions>();
        services.Configure<IndexingOptions>(indexingOptionsSection);

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);

        // Indexing
        services.RegisterSearchDependencies(indexingOptions);

        // Automapper
        services.RegisterMapperDependencies();
    }
}
