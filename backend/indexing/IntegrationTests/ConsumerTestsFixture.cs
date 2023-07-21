using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class ConsumerTestsFixture
{
    public ITestHarness TestHarness { get; private set; }

    public ConsumerTestsFixture()
    {
        TestHarness = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .ConfigureServices(services => services
                    .AddMassTransitTestHarness()))
            .Services
            .GetTestHarness();
    }
}
