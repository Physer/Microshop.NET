using Application.Interfaces.Indexing;
using Application.Interfaces.ProductsClient;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using Search;

namespace Tests.Indexing;

internal class IndexingServiceBuilder
{
    public readonly IMapper MapperMock;
    private readonly IProductsClient _productsClientMock;
    private readonly MeilisearchClientFake _meilisearchClientFake;

    public IndexingServiceBuilder()
    {
        MapperMock = Substitute.For<IMapper>();
        _productsClientMock = Substitute.For<IProductsClient>();
        _meilisearchClientFake = new MeilisearchClientFake(new Fixture().Create<Uri>().ToString());
    }

    public IIndexingService Build() => new IndexingService(MapperMock, _productsClientMock, _meilisearchClientFake);
}
