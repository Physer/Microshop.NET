namespace Application.Interfaces.Indexing;

public interface IIndexingClient
{
    Task AddOrUpdateDocumentsAsync<T>(IEnumerable<T> documentsToIndex);
    Task<IEnumerable<T>> GetAllDocumentsAsync<T>();
}
