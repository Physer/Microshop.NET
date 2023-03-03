using AutoMapper;
using Domain;
using NSubstitute;
using Persistence;

namespace Tests.Repositories;

internal class ProductRepositoryBuilder
{
    private readonly IMapper _mapperMock;
    private readonly IEnumerable<ProductData> _data;

    public ProductRepositoryBuilder()
    {
        _mapperMock = Substitute.For<IMapper>();
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
        _mapperMock.Map<IEnumerable<Product>>(Arg.Any<IEnumerable<ProductData>>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingDatabaseEntryToProductReturns(Product output)
    {
        _mapperMock.Map<Product>(Arg.Any<ProductData>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingProductsToDatabaseEntriesReturns(IEnumerable<ProductData> output)
    {
        _mapperMock.Map<IEnumerable<ProductData>>(Arg.Any<IEnumerable<Product>>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepositoryBuilder WithMappingProductToDatabaseEntryReturns(ProductData output)
    {
        _mapperMock.Map<ProductData>(Arg.Any<Product>()).ReturnsForAnyArgs(output);

        return this;
    }

    public ProductRepository Build() => new(_mapperMock, _data);
}
