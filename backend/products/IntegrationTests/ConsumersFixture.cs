using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class ConsumersFixture<TConsumer> where TConsumer : class, IConsumer
{
    public ITestHarness TestHarness { get; private set; }

    public ConsumersFixture()
    {
        var applicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .ConfigureServices(services => services
                    .AddMassTransitTestHarness(busRegistrator => busRegistrator
                        .AddConsumer<TConsumer>())));
        TestHarness = applicationFactory.Services.GetTestHarness();
    }
}
