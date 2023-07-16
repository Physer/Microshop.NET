using Application.Interfaces.Indexing;
using Application.Models;
using Domain;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class PricesGeneratedConsumer : IConsumer<PricesGenerated>
{
    private readonly ILogger<PricesGeneratedConsumer> _logger;
    private readonly IIndexingService _indexingService;

    public PricesGeneratedConsumer(ILogger<PricesGeneratedConsumer> logger,
        IIndexingService indexingService)
    {
        _logger = logger;
        _indexingService = indexingService;
    }

    public async Task Consume(ConsumeContext<PricesGenerated> context)
    {
        _logger.LogInformation("Indexing service - Consuming {messageId} from the {consumerName}", context?.MessageId, nameof(ProductsGeneratedConsumer));
        await _indexingService.IndexAsync<Price, IndexableProduct>(context?.Message?.Prices);
    }
}
