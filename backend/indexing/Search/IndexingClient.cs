using Application.Interfaces.Indexing;

namespace Search;

public class IndexingClient : IIndexingClient
{
    internal readonly IMicroshopIndex _microshopIndex;

    public IndexingClient(IMicroshopIndex microshopIndex) => _microshopIndex = microshopIndex;

    public async Task<IEnumerable<T>> GetAllDocumentsAsync<T>() => await _microshopIndex.GetAllDocumentsAsync<T>();
    public async Task AddOrUpdateDocumentsAsync<T>(IEnumerable<T> documentsToIndex) => await _microshopIndex.AddOrUpdateDocumentsAsync(documentsToIndex);
}
