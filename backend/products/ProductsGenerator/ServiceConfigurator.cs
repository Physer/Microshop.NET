using Application.Interfaces.Generator;
using Application.Interfaces.Repositories;
using Application.Options;
using Generator;
using Mapper;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

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

        // Persistence
        services.AddSingleton<IRepository, ProductRepository>();

        // Generator
        services.AddTransient<IProductGenerator, ProductGenerator>();

        // Mapper
        services.AddAutoMapper(typeof(ProductProfile));

        // Messaging
        services.RegisterMessagingDependencies(servicebusOptions);
    }
}
