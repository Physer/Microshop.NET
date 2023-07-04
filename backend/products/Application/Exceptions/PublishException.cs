using System.Runtime.Serialization;

namespace Application.Exceptions;

public class PublishException : Exception
{
    public PublishException()
    {
    }

    public PublishException(string? message) : base(message)
    {
    }

    public PublishException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PublishException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
