using Application.Authentication;
using Authentication.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Authentication;

public static class DependencyRegistrator
{
    public static void RegisterAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationOptionsSection = configuration.GetSection(AuthenticationOptions.ConfigurationEntry);
        var authenticationOptions = authenticationOptionsSection.Get<AuthenticationOptions>();
        ArgumentNullException.ThrowIfNull(authenticationOptions, nameof(authenticationOptions));

        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(configuration => configuration.BaseAddress = new Uri(authenticationOptions.BaseUrl));
        services.AddScoped<ITokenParser, TokenParser>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }
}
