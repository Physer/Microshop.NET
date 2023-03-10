using Application.Interfaces.ProductsClient;
using AutoMapper;
using Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ProductsClient.Contracts;

namespace ProductsClient;

public class ProductsAmqpClient : IProductsClient
{
    private readonly IRequestClient<GetProductsRequest> _requestClient;
    private readonly IMapper _mapper;

    public ProductsAmqpClient(IMapper mapper, IServiceScopeFactory serviceScopeFactory)
    {
        _mapper = mapper;
        using var scope = serviceScopeFactory.CreateScope();
        _requestClient = scope.ServiceProvider.GetRequiredService<IRequestClient<GetProductsRequest>>();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _requestClient.GetResponse<GetProductsResponse>(new GetProductsRequest(), cancellationToken);
        return response?.Message?.Products?.Select(_mapper.Map<Product>) ?? Enumerable.Empty<Product>();
    }
}
