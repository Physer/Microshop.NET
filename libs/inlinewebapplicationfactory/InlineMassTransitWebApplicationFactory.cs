using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MassTransit;

namespace InlineWebApplicationFactory;

internal class InlineMassTransitWebApplicationFactory<TProgram, TOptions> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly IDictionary<string, string?> _configuration;
    private readonly TOptions _options;
    private readonly string _optionsSectionKey;
    private readonly Action<IBusRegistrationConfigurator> _busConfigurator;

    public InlineMassTransitWebApplicationFactory(IDictionary<string, string?> configuration,
        TOptions options,
        string optionsSectionKey,
        Action<IBusRegistrationConfigurator> busConfigurator)
    {
        _configuration = configuration;
        _options = options;
        _optionsSectionKey = optionsSectionKey;
        _busConfigurator = busConfigurator;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_configuration).Build();
        var servicebusOptionsSection = inMemoryConfiguration.GetSection(_optionsSectionKey);
        var servicebusOptions = servicebusOptionsSection.Get(_options!.GetType());

        builder.UseConfiguration(inMemoryConfiguration);
        builder.ConfigureServices(services => services.AddMassTransitTestHarness(_busConfigurator));
    }
}
