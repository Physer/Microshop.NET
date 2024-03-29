﻿using Application.Options;
using MassTransit;
using Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDictionary<string, string?> _configuration;

    public ApiWebApplicationFactory(IDictionary<string, string?> configuration) => _configuration = configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_configuration).Build();
        var servicebusOptionsSection = inMemoryConfiguration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>();

        builder.UseConfiguration(inMemoryConfiguration);
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(IntegrationAuthenticationHandler.IntegrationTestSchema).AddScheme<IntegrationAuthenticationOptions, IntegrationAuthenticationHandler>(IntegrationAuthenticationHandler.IntegrationTestSchema, _ => { });
            services.AddMassTransitTestHarness(cfg => cfg.ConfigureBusRegistration(servicebusOptions));
        });
    }
}
