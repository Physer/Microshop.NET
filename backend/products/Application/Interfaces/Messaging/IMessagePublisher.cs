namespace Application.Interfaces.Messaging;

public interface IMessagePublisher
{
    Task PublishMessage();
}
