using AutoMapper;
using Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ProductsClient;
using ProductsClient.Contracts;
using RabbitMQ.Client.Exceptions;

namespace Tests.Builders;

internal class ProductsClientBuilder
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRequestClient<GetProductsRequest> _requestClient;

    public ProductsClientBuilder()
    {
        _mapper = Substitute.For<IMapper>();
        _requestClient = Substitute.For<IRequestClient<GetProductsRequest>>();

        var serviceProvider = Substitute.For<IServiceProvider>();
        var serviceScope = Substitute.For<IServiceScope>();
        serviceScope.ServiceProvider.Returns(serviceProvider);
        serviceProvider.GetService(typeof(IRequestClient<GetProductsRequest>)).Returns(_requestClient);

        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        _serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceScope.ServiceProvider.GetRequiredService(typeof(IRequestClient<GetProductsRequest>)).Returns(_requestClient);
    }

    public ProductsClientBuilder WithRequestClientUnreachable()
    {
        _requestClient.When(client => client.GetResponse<GetProductsResponse>(new GetProductsRequest(), CancellationToken.None)).Do(_ => { throw new BrokerUnreachableException(new Exception()); });

        return this;
    }

    public ProductsClientBuilder WithRequestClientTimingOut()
    {
        _requestClient.When(client => client.GetResponse<GetProductsResponse>(new GetProductsRequest(), CancellationToken.None)).Do(_ => { throw new RequestTimeoutException(); });

        return this;
    }

    public ProductsClientBuilder WithMapperMappingProduct(ProductResponse responseProduct, Product productToMapTo)
    {
        _mapper.Map<Product>(responseProduct).ReturnsForAnyArgs(productToMapTo);

        return this;
    }

    public ProductsClientBuilder WithRequestClientReturningProducts(IEnumerable<ProductResponse>? responseProducts)
    {
        var response = Substitute.For<Response<GetProductsResponse>>();
        response.Message.Returns(new GetProductsResponse { Products = responseProducts });

        _requestClient.GetResponse<GetProductsResponse>(new GetProductsRequest(), CancellationToken.None).Returns(response);

        return this;
    }

    public ProductsAmqpClient Build() => new(_mapper, _serviceScopeFactory);
}
