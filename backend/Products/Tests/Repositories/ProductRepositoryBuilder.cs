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

    public ProductRepositoryBuilder WithProductData(ProductData data) => WithProductDataContains(new[] { data });

    public ProductRepositoryBuilder WithProductData(IEnumerable<ProductData> data) => WithProductDataContains(data);

    private ProductRepositoryBuilder WithProductDataContains(IEnumerable<ProductData> data)
    {
        _data.GetEnumerator().Returns(_ => data.GetEnumerator());

        return this;
    }

    public ProductRepositoryBuilder WithMappingDatabaseEntriesToProductsReturns(IEnumerable<Product> output)
    {
        _productMapperMock.MapDatabaseEntriesToProducts(Arg.Any<IEnumerable<ProductData>>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingDatabaseEntryToProductReturns(Product output)
    {
        _productMapperMock.MapDatabaseEntryToProduct(Arg.Any<ProductData>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingProductsToDatabaseEntriesReturns(IEnumerable<ProductData> output)
    {
        _productMapperMock.MapProductsToDatabaseEntries(Arg.Any<IEnumerable<Product>>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingProductToDatabaseEntryReturns(ProductData output)
    {
        _productMapperMock.MapProductToDatabaseEntry(Arg.Any<Product>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepository Build() => new(_productMapperMock, _data);
}
