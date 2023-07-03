namespace Application.Interfaces.Messaging;

public interface IMessagePublisher
{
    Task PublishMessage<T>(T message) where T : class;
}
