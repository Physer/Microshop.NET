using Application.Options;
using Messaging;

namespace Service;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var servicebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servicebusOptionsSection);

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
