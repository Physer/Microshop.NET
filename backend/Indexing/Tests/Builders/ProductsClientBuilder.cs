using AutoMapper;
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
        _requestClient.When(client => client.GetResponse<GetProductsResponse>(new GetProductsRequest(), Arg.Any<CancellationToken>())).Do(_ => { throw new BrokerUnreachableException(Arg.Any<Exception>()); });

        return this;
    }

    public ProductsAmqpClient Build() => new(_mapper, _serviceScopeFactory);
}
