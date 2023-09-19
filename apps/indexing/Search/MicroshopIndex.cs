using Application.Interfaces.Indexing;
using Meilisearch;
using Index = Meilisearch.Index;

namespace Search;

internal class MicroshopIndex : IIndex
{
    private readonly Index _index;

    public MicroshopIndex(MeilisearchClient meilisearchClient) => _index = meilisearchClient.Index("products");

    public async Task AddOrUpdateDocumentsAsync<T>(IEnumerable<T> documentsToIndex) => await _index.UpdateDocumentsAsync(documentsToIndex);
    public async Task ClearIndex() => await _index.DeleteAllDocumentsAsync();
}
