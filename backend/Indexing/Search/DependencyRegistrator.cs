using Application.Interfaces.Indexing;
using Application.Options;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;

namespace Search;

public static class DependencyRegistrator
{
    public static void RegisterSearchDependencies(this IServiceCollection services, IndexingOptions? indexingOptions)
    {
        if (indexingOptions is null)
            return;

        services.AddSingleton<IIndexingService, IndexingService>();
        services.AddSingleton(_ => new MeilisearchClient(indexingOptions.BaseUrl, indexingOptions.ApiKey));
    }
}
