using MassTransit;
using Messaging.Messages;

namespace Messaging;

internal class PricesGeneratedConsumer : IConsumer<PricesGenerated>
{
    public async Task Consume(ConsumeContext<PricesGenerated> context) => await Task.CompletedTask;
}
