using Application.Exceptions;
using Application.Interfaces.Messaging;
using Domain;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging.Publishers;

internal class PricesGeneratedMessagePublisher : IPricesGeneratedMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PricesGeneratedMessagePublisher> _logger;

    internal Guid? _messageId;

    public PricesGeneratedMessagePublisher(IPublishEndpoint publishEndpoint, 
        ILogger<PricesGeneratedMessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Guid?> PublishMessage(IEnumerable<Price> prices)
    {
        _logger.LogInformation("Publishing message from {publisher}", nameof(PricesGeneratedMessagePublisher));
        await _publishEndpoint.Publish(new PricesGenerated(prices), x =>
        {
            _messageId = x.MessageId;
        });

        if (_messageId == Guid.Empty)
            throw new MessagePublishingException($"Unable to publish a message using the {nameof(PricesGeneratedMessagePublisher)}");
        return _messageId;
    }
}
