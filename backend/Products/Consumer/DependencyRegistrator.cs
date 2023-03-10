using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer;

public static class DependencyRegistrator
{
    public static void RegisterConsumerDependencies(this IServiceCollection services, ProductsOptions? productsOptions)
    {
        if (productsOptions is null)
            return;

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(productsOptions.BaseUrl, "/", hostConfigurator => {
                    hostConfigurator.Username(productsOptions.ManagementUsername);
                    hostConfigurator.Password(productsOptions.ManagementPassword);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
    }
}
