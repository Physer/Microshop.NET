using MassTransit;
using Messaging.Messages;

namespace Messaging;

internal class PricesGeneratedConsumer : IConsumer<PricesGenerated>
{
    public Task Consume(ConsumeContext<PricesGenerated> context) => throw new NotImplementedException("This method still has to be implemented and index the prices properly");
}
