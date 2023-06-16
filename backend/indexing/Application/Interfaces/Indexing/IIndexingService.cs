namespace Application.Interfaces.Indexing;

public interface IIndexingService
{
    Task IndexProductsAsync(CancellationToken cancellationToken);
}
