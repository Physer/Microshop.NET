using Application.Interfaces.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(IPublishEndpoint publishEndpoint,
        ILogger<MessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishMessage<T>(T message) where T : class
    {
        _logger.LogInformation("Publishing message {messageName}", message.GetType().Name);
        await _publishEndpoint.Publish(message);
    }
}
