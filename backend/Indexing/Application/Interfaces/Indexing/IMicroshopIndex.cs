namespace Application.Interfaces.Indexing;

public interface IMicroshopIndex
{
    Task AddDocumentsAsync<T>(IEnumerable<T> documentsToIndex);
    Task DeleteAllDocumentsAsync();
}
