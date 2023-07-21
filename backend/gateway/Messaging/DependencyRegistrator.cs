using Application.Interfaces.Messaging;
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
        services.AddScoped<IGenerateProductsPublisher, GenerateProductsPublisher>();
        services.AddMassTransit(busConfigurator =>
        {
            if (servicebusOptions is null)
                throw new ArgumentException("Invalid servicebus options", nameof(servicebusOptions));

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
