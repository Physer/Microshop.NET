using Microsoft.Extensions.DependencyInjection;

namespace Mapper;

public static class DependencyRegistrator
{
    public static void RegisterMapperDependencies(this IServiceCollection services) => services.AddAutoMapper(typeof(DependencyRegistrator));
}
