using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Mapper;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterMapperDependencies(this IServiceCollection services) => services.AddAutoMapper(typeof(ProductProfile));
}
