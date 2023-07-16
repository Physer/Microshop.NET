namespace Application.Exceptions;

public class MessagePublishingException : Exception
{
    public MessagePublishingException(string? message) : base(message) { }
}
