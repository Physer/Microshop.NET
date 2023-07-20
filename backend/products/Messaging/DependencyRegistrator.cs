using Application.Interfaces.Messaging;
using Application.Options;
using MassTransit;
using Messaging.Publishers;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntegrationTests")]
namespace Messaging;

public static class DependencyRegistrator
{
    public static void RegisterMessagingDependencies(this IServiceCollection services, ServicebusOptions? servicebusOptions)
    {
        services.AddScoped<IProductsGeneratedMessagePublisher, ProductsGeneratedMessagePublisher>();
        services.AddMassTransit(busConfigurator => busConfigurator.ConfigureBusRegistration(servicebusOptions));
    }

    internal static void ConfigureBusRegistration(this IBusRegistrationConfigurator busConfigurator, ServicebusOptions? servicebusOptions)
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
    }
}
