using Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Web")]
namespace Authentication;

public static class DependencyRegistrator
{
    public static void RegisterAuthenticationDependencies(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>();
    }
}
