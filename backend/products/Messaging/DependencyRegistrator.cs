using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Messaging;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterMessagingDependencies(this IServiceCollection services, ServicebusOptions? servicebusOptions)
    {
        if (servicebusOptions is null)
            throw new ArgumentNullException(nameof(servicebusOptions), "Invalid servicebus options");

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter("products", false));
            busConfigurator.AddConsumer<GenerateProductsConsumer>();
            busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(servicebusOptions.BaseUrl, (ushort)servicebusOptions.Port, "/", hostConfigurator =>
                {
                    hostConfigurator.Username(servicebusOptions.ManagementUsername);
                    hostConfigurator.Password(servicebusOptions.ManagementPassword);
                });
                factoryConfigurator.ConfigureEndpoints(context);
            });
        });
    }
}
