using Application.Interfaces.Generator;
using Application.Options;
using Generator;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsGenerator;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Options
        var servciebusOptionsSection = configuration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servciebusOptionsSection.Get<ServicebusOptions>();
        services.Configure<ServicebusOptions>(servciebusOptionsSection);
        var dataOptionsSection = configuration.GetSection(DataOptions.ConfigurationEntry);
        services.Configure<DataOptions>(dataOptionsSection);

        // Generator
        services.AddTransient<IProductGenerator, ProductGenerator>();

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
