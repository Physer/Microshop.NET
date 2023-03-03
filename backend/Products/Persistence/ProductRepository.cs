using Application;
using AutoMapper;
using Domain;

namespace Persistence;

public class ProductRepository : IRepository
{
    private readonly HashSet<ProductData> _databaseProducts;
    private readonly IMapper _mapper;

    public ProductRepository(IMapper mapper, IEnumerable<ProductData> data)
    {
        _mapper = mapper;
        _databaseProducts = data?.ToHashSet() ?? new HashSet<ProductData>();
    }

    public Product? GetProductById(Guid id)
    {
        var databaseProduct = _databaseProducts.FirstOrDefault(entry => entry.Id.Equals(id));
        return databaseProduct is not null ? _mapper.Map<Product>(databaseProduct) : null;
    }

    public IEnumerable<Product> GetProducts() => _mapper.Map<IEnumerable<Product>>(_databaseProducts);

    public void CreateProduct(Product product) => _databaseProducts.Add(_mapper.Map<ProductData>(product));

    public void CreateProducts(IEnumerable<Product> products) => _databaseProducts.UnionWith(_mapper.Map<IEnumerable<ProductData>>(products));

}
