using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class ApiTestsFixture
{
    public WebApplicationFactory<Program> ApplicationFactory { get; private set; }

    public ApiTestsFixture()
    {
        ApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .ConfigureServices(services => services
                    .AddMassTransitTestHarness()));
    }
}
