using Application.Interfaces.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Messaging;

public class MessagePublisher<T> : IMessagePublisher<T> where T : IMessage
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MessagePublisher<T>> _logger;

    public MessagePublisher(IPublishEndpoint publishEndpoint,
        ILogger<MessagePublisher<T>> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishMessage(T message)
    {
        _logger.LogInformation("Publishing message {messageName} from the {publisherName}", message.GetType().Name, nameof(MessagePublisher<T>));
        await _publishEndpoint.Publish(message);
    }
}
