using Application.Interfaces.Indexing;
using Application.Options;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Search;

[ExcludeFromCodeCoverage]
public static class DependencyRegistrator
{
    public static void RegisterSearchDependencies(this IServiceCollection services, IndexingOptions? indexingOptions)
    {
        if (indexingOptions is null)
            return;

        var meilisearchClient = new MeilisearchClient(indexingOptions.BaseUrl, indexingOptions.ApiKey);
        services.AddSingleton(_ => meilisearchClient);
        services.AddSingleton<IIndexingService, IndexingService>();
        services.AddSingleton<IIndex, MicroshopIndex>(_ => new MicroshopIndex(meilisearchClient));
    }
}
