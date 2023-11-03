using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace InlineWebApplicationFactory;

public class InlineWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
    private readonly IDictionary<string, string?> _configuration;

    public InlineWebApplicationFactory(IDictionary<string, string?> configuration) => _configuration = configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var inMemoryConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_configuration).Build();

        builder.UseConfiguration(inMemoryConfiguration);
    }
}
