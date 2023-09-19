using Application.Interfaces.Indexing;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class ClearDataConsumer : IConsumer<ClearData>
{
    private readonly ILogger<ClearDataConsumer> _logger;
    private readonly IIndexingService _indexingService;

    public ClearDataConsumer(ILogger<ClearDataConsumer> logger,
        IIndexingService indexingService)
    {
        _logger = logger;
        _indexingService = indexingService;
    }

    public async Task Consume(ConsumeContext<ClearData> context)
    {
        _logger.LogInformation("Indexing service - Consuming {messageId} from the {consumerName}", context?.MessageId, nameof(ProductsGeneratedConsumer));
        await _indexingService.ClearIndex();
    }
}
