using Application.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyRegistrator
{
    public static void RegisterApplicationDependencies(this IServiceCollection services) => services.AddScoped<ITokenRetriever, TokenRetriever>();
}