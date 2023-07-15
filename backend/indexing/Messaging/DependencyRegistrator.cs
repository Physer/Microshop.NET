﻿using Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntegrationTests")]
namespace Messaging;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterMessagingDependencies(this IServiceCollection services, ServicebusOptions? servicebusOptions)
    {
        if (servicebusOptions is null)
            return;

        services.AddMassTransit(busConfigurator => busConfigurator.ConfigureBusRegistration(servicebusOptions));
    }

    internal static void ConfigureBusRegistration(this IBusRegistrationConfigurator busConfigurator, ServicebusOptions servicebusOptions)
    {
        busConfigurator.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter("indexing", false));
        busConfigurator.AddConsumer<ProductsGeneratedConsumer>();
        busConfigurator.AddConsumer<PricesGeneratedConsumer>();
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
