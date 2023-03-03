namespace Application;

public interface IProductsClient
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
}
