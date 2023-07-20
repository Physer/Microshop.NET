using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging;

public static class DependencyRegistrator
{
    public static void RegisterMessagingDependencies(this IServiceCollection services, ServicebusOptions servicebusOptions) => services.AddMassTransit(busConfigurator => 
    busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
    {
        busConfigurator.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter("products", false));
        busConfigurator.AddConsumer<GenerateProductsConsumer>();
        factoryConfigurator.Host(servicebusOptions.BaseUrl, (ushort)servicebusOptions.Port, "/", hostConfigurator =>
        {
            hostConfigurator.Username(servicebusOptions.ManagementUsername);
            hostConfigurator.Password(servicebusOptions.ManagementPassword);
        });
        factoryConfigurator.ConfigureEndpoints(context);
    }));
}
