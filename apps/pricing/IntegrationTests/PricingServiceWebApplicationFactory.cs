using Application.Options;
using MassTransit;
using Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests;

internal class PricingServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDictionary<string, string?> _configuration;

    public PricingServiceWebApplicationFactory(IDictionary<string, string?> configuration) => _configuration = configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_configuration).Build();
        var servicebusOptionsSection = inMemoryConfiguration.GetSection(ServicebusOptions.ConfigurationEntry);
        var servicebusOptions = servicebusOptionsSection.Get<ServicebusOptions>(); 

        builder.UseConfiguration(inMemoryConfiguration);
        builder.ConfigureServices(services => services.AddMassTransitTestHarness(cfg => cfg.ConfigureBusRegistration(servicebusOptions)));
    }
}
