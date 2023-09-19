namespace Application.Interfaces.Indexing;

public interface IIndex
{
    Task AddOrUpdateDocumentsAsync<T>(IEnumerable<T> documentsToIndex);
    Task ClearIndex();
}
