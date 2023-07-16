using Application.Interfaces.Indexing;

namespace Search;

public class IndexingClient : IIndexingClient
{
    internal readonly IMicroshopIndex _microshopIndex;

    public IndexingClient(IMicroshopIndex microshopIndex)
    {
        _microshopIndex = microshopIndex;
    }

    public async Task DeleteAllDocumentsAsync() => await _microshopIndex.DeleteAllDocumentsAsync();
    public async Task AddDocumentsAsync<T>(IEnumerable<T> documentsToIndex) => await _microshopIndex.AddDocumentsAsync(documentsToIndex);
    public async Task<IEnumerable<T>> GetAllDocumentsAsync<T>() => await _microshopIndex.GetAllDocumentsAsync<T>();
}
