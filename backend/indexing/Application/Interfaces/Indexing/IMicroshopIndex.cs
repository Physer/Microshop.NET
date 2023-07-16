namespace Application.Interfaces.Indexing;

public interface IMicroshopIndex
{
    Task AddOrUpdateDocumentsAsync<T>(IEnumerable<T> documentsToIndex);
    Task<IEnumerable<T>> GetAllDocumentsAsync<T>();
}
