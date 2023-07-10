using Application.Options;
using Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntegrationTests")]
namespace API;

[ExcludeFromCodeCoverage]
public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var servciebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servciebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servciebusOptionsSection);

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
