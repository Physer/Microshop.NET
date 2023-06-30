using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProductsClient.Contracts;

namespace Consumer;

public class ProductsRequestConsumer : IConsumer<GetProductsRequest>
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;
    private readonly ILogger<ProductsRequestConsumer> _logger;

    public ProductsRequestConsumer(IMapper mapper,
        IRepository repository,
        ILogger<ProductsRequestConsumer> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<GetProductsRequest> context)
    {
        _logger.LogInformation("Received request {id} for retrieving products", context.RequestId);
        var products = _repository.GetProducts();
        var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
        await context.RespondAsync(new GetProductsResponse(productResponses));
        _logger.LogInformation("Send message {messageId} with {productResponsesAmount} products", context.MessageId, productResponses.Count());
    }
}
