using Application.Interfaces.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Generator;

public static class DependencyRegistrator
{
    public static void RegisterGeneratorDependencies(this IServiceCollection services) => services.AddSingleton<IPriceGenerator, PriceGenerator>();
}
