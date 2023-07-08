using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("IntegrationTests")]
namespace API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services) => ServiceConfigurator.ConfigureServices(_configuration, services);

    public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/products", Endpoints.GenerateProducts);
        });
    }
}
