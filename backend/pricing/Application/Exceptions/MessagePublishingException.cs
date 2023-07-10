using System.Runtime.Serialization;

namespace Application.Exceptions;

public class MessagePublishingException : Exception
{
    public MessagePublishingException()
    {
    }

    public MessagePublishingException(string? message) : base(message)
    {
    }

    public MessagePublishingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MessagePublishingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
