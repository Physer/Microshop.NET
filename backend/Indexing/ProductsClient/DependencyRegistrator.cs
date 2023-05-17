using Application.Interfaces.ProductsClient;
using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ProductsClient;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterAmqpDependencies(this IServiceCollection services, ServicebusOptions? productOptions)
    {
        if (productOptions is null)
            return;

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(productOptions.BaseUrl, (ushort)productOptions.Port, "/", hostConfigurator => {
                    hostConfigurator.Username(productOptions.ManagementUsername);
                    hostConfigurator.Password(productOptions.ManagementPassword);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
        services.AddSingleton<IProductsClient, ProductsAmqpClient>();
    }
}
