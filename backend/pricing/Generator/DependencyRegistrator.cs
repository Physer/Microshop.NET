using Application.Interfaces.Generator;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Generator;

public static class DependencyRegistrator
{
    public static void RegisterGeneratorDependencies(this IServiceCollection services) => services.AddSingleton<IPriceGenerator, PriceGenerator>();
}
