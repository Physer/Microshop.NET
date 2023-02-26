using Domain;
using NSubstitute;
using Persistence;

namespace Tests.Repositories;

internal class ProductRepositoryBuilder
{
    private readonly IProductMapper _productMapperMock;
    private readonly List<ProductData> _data;

    public ProductRepositoryBuilder()
    {
        _productMapperMock = Substitute.For<IProductMapper>();
        _data = new List<ProductData>();
    }

    public ProductRepositoryBuilder WithProductData(IEnumerable<ProductData> data)
    {
        _data.AddRange(data);

        return this;
    }

    public ProductRepositoryBuilder WithMapDatabaseEntryReturns(ProductData input, Product output)
    {
        _productMapperMock.MapDatabaseEntryToProduct(input).Returns(output);

        return this;
    }

    public ProductRepository Build() => new(_productMapperMock, _data);
}
