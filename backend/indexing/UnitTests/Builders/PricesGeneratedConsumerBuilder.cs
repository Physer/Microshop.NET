using Application.Interfaces.Indexing;
using Domain;
using MassTransit;
using Messaging.Messages;
using Messaging;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Builders;

internal class PricesGeneratedConsumerBuilder
{
    internal readonly IIndexingService _indexingService;
    internal readonly ILogger<PricesGeneratedConsumer> _logger;

    internal ConsumeContext<PricesGenerated>? _consumeContext;

    public PricesGeneratedConsumerBuilder()
    {
        _indexingService = Substitute.For<IIndexingService>();
        _logger = Substitute.For<ILogger<PricesGeneratedConsumer>>();
        _consumeContext = Substitute.For<ConsumeContext<PricesGenerated>>();
    }

    public PricesGeneratedConsumerBuilder WithReceivingPrices(IEnumerable<Price> prices)
    {
        _consumeContext!.Message.Returns(new PricesGenerated(prices));

        return this;
    }

    public PricesGeneratedConsumerBuilder WithMessage(Guid messageId)
    {
        _consumeContext!.MessageId.Returns(messageId);

        return this;
    }

    public PricesGeneratedConsumerBuilder WithoutConsumeContext()
    {
        _consumeContext = null;

        return this;
    }

    public PricesGeneratedConsumer Build() => new(_logger, _indexingService);
}
