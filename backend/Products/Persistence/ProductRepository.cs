using Application;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    private readonly HashSet<ProductData> _databaseProducts;
    private readonly IProductMapper _mapper;

    public ProductRepository(IProductMapper productMapper, IEnumerable<ProductData> data)
    {
        _mapper = productMapper;
        _databaseProducts = data?.ToHashSet() ?? new HashSet<ProductData>();
    }

    public Product? GetProductById(Guid id)
    {
        var databaseProduct = _databaseProducts.FirstOrDefault(entry => entry.Id.Equals(id));
        return databaseProduct is not null ? _mapper.MapDatabaseEntryToProduct(databaseProduct) : null;
    }

    public IEnumerable<Product> GetProducts() => _databaseProducts.Select(_mapper.MapDatabaseEntryToProduct);

    public void CreateProduct(Product product) => _databaseProducts.Add(_mapper.MapProductToDatabaseEntry(product));

    public void CreateProducts(IEnumerable<Product> products) => _databaseProducts.UnionWith(products.Select(_mapper.MapProductToDatabaseEntry));

}
