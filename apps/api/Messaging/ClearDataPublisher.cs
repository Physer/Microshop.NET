using Application.Interfaces.Messaging;
using MassTransit;
using Messaging.Messages;

namespace Messaging;

public class ClearDataPublisher : IClearDataPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ClearDataPublisher(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task PublishMessage() => await _publishEndpoint.Publish(new ClearData());
}
