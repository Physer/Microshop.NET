namespace Application.Interfaces.Indexing;

public interface IIndexingService
{
    Task ClearIndex();
    Task IndexAsync<TInput, TIndexableModel>(IEnumerable<TInput>? dataToIndex);
}
