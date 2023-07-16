namespace Application.Interfaces.Indexing;

public interface IIndexingService
{
    Task IndexAsync<TInput, TIndexableModel>(IEnumerable<TInput>? dataToIndex);
}
