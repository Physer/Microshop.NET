using Application.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Web")]
namespace Authentication;

public static class DependencyRegistrator
{
    public static void RegisterAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationBaseUrl = configuration.GetSection("Authentication").GetValue<string>("BaseUrl");
        if(string.IsNullOrWhiteSpace(authenticationBaseUrl))
            throw new ArgumentNullException(nameof(authenticationBaseUrl), "Invalid authentication options");

        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(clientConfiguration => clientConfiguration.BaseAddress = new Uri(authenticationBaseUrl));
    }
}
