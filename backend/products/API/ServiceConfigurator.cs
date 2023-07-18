using Application.Options;
using Generator;
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
        var servicebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servicebusOptionsSection);

        // Generator
        services.RegisterGeneratorDependencies();

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
