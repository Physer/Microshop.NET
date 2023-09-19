using System.Runtime.Serialization;

namespace Application.Exceptions;

/// <summary>
/// This exception is thrown when the API does not return a succesful response
/// </summary>
public class MicroshopApiException : Exception
{
    public MicroshopApiException() { }

    public MicroshopApiException(string? message) : base(message) { }

    public MicroshopApiException(string? message, Exception? innerException) : base(message, innerException) { }

    protected MicroshopApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
