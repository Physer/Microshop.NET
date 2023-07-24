using Application.Options;
using Generator;
using Messaging;

namespace API;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var servicebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servicebusOptionsSection);

        // Generator
        services.RegisterGeneratorDependencies();

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
