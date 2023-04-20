using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using MassTransit;
using ProductsClient.Contracts;

namespace Consumer;

public class ProductsRequestConsumer : IConsumer<GetProductsRequest>
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public ProductsRequestConsumer(IMapper mapper,
        IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<GetProductsRequest> context)
    {
        var products = _repository.GetProducts();
        var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
        await context.RespondAsync(new GetProductsResponse(productResponses));
    }
}
