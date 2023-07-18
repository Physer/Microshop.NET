using Application.Interfaces.Generator;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Generator;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterGeneratorDependencies(this IServiceCollection services) => services.AddSingleton<IProductGenerator, ProductGenerator>();
}
