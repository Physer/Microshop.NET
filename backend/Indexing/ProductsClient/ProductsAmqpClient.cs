using Application.Interfaces.ProductsClient;
using AutoMapper;
using Domain;
using MassTransit;
using ProductsClient.Contracts;

namespace ProductsClient;

public class ProductsAmqpClient : IProductsClient
{
    private readonly IRequestClient<GetProductsRequest> _requestClient;
    private readonly IMapper _mapper;

    public ProductsAmqpClient(IRequestClient<GetProductsRequest> requestClient, 
        IMapper mapper)
    {
        _requestClient = requestClient;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var response = await _requestClient.GetResponse<GetProductsResponse>(new GetProductsRequest());
        return response.Message.Products.Select(_mapper.Map<Product>);
    }
}
