namespace Application.Interfaces.Indexing;

public interface IIndexingClient
{
    Task AddDocumentsAsync<T>(IEnumerable<T> documentsToIndex);
    Task DeleteAllDocumentsAsync();
    Task<IEnumerable<T>> GetAllDocumentsAsync<T>();
}
