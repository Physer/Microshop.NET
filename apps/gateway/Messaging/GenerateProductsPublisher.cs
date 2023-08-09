using Application.Interfaces.Messaging;
using MassTransit;
using Messaging.Messages;

namespace Messaging;

public class GenerateProductsPublisher : IGenerateProductsPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public GenerateProductsPublisher(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task PublishMessage() => await _publishEndpoint.Publish(new GenerateProducts());
}
