namespace Application.Interfaces.Messaging;

public interface IMessagePublisher<T> where T : IMessage
{
    Task PublishMessage(T message);
}
