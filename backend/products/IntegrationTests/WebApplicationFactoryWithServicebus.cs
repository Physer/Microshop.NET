using Application.Options;
using MassTransit;
using Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

internal class WebApplicationFactoryWithServicebus<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly ServicebusOptions? _servicebusOptions;

    public WebApplicationFactoryWithServicebus(ServicebusOptions? servicebusOptions) => _servicebusOptions = servicebusOptions;

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureServices(services => services
            .AddMassTransitTestHarness(config => config
                .ConfigureBusRegistration(_servicebusOptions)));
}
