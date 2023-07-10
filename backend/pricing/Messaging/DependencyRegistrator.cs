using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging;

public static class DependencyRegistrator
{
    public static void RegisterMessagingDependencies(this IServiceCollection services, ServicebusOptions? servicebusOptions)
    {
        if (servicebusOptions is null)
            return;

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<ProductsGeneratedConsumer>();
            busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(servicebusOptions.BaseUrl, (ushort)servicebusOptions.Port, "/", hostConfigurator => {
                    hostConfigurator.Username(servicebusOptions.ManagementUsername);
                    hostConfigurator.Password(servicebusOptions.ManagementPassword);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
    }
}
