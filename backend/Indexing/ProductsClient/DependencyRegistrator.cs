using Application.Interfaces.ProductsClient;
using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsClient;

public static class DependencyRegistrator
{
    public static void RegisterAmqpDependencies(this IServiceCollection services, ProductsOptions? productOptions)
    {
        if (productOptions is null)
            return;

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(productOptions.BaseUrl, "/", hostConfigurator => {
                    hostConfigurator.Username(productOptions.ManagementUsername);
                    hostConfigurator.Password(productOptions.ManagementPassword);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<IProductsClient, ProductsAmqpClient>();
    }
}
