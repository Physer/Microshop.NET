using Application.Models;
using Application.Queries.GetProducts;
using AutoMapper;
using MassTransit;
using MediatR;
using ProductsClient.Contracts;

namespace Consumer;

public class ProductsRequestConsumer : IConsumer<GetProductsRequest>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsRequestConsumer(IMediator mediator, 
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<GetProductsRequest> context)
    {
        var products = await _mediator.Send(new GetProductsQuery());
        var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
        await context.RespondAsync(new GetProductsResponse(productResponses));
    }
}
