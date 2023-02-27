using Domain;
using NSubstitute;
using Persistence;

namespace Tests.Repositories;

internal class ProductRepositoryBuilder
{
    private readonly IProductMapper _productMapperMock;
    private readonly IEnumerable<ProductData> _data;

    public ProductRepositoryBuilder()
    {
        _productMapperMock = Substitute.For<IProductMapper>();
        _data = Substitute.For<IEnumerable<ProductData>>();
    }

    public ProductRepositoryBuilder WithProductData(IEnumerable<ProductData> data)
    {
        _data.GetEnumerator().Returns(_ => { return data.GetEnumerator(); });

        return this;
    }

    public ProductRepositoryBuilder WithMappingDatabaseEntryToProductReturns(ProductData input, Product output)
    {
        _productMapperMock.MapDatabaseEntryToProduct(input).Returns(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingProductToDatabaseEntryReturns(Product input, ProductData output)
    {
        _productMapperMock.MapProductToDatabaseEntry(input).Returns(output);

        return this;
    }

    public ProductRepository Build() => new(_productMapperMock, _data);
}
