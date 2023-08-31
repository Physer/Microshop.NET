using System.Runtime.Serialization;

namespace Application.Exceptions;

/// <summary>
/// This exception indicates something went wrong whilst trying to authenticate the user
/// </summary>
public class AuthenticationException : Exception
{
    public AuthenticationException() { }

    public AuthenticationException(string? message) : base(message) { }

    public AuthenticationException(string? message, Exception? innerException) : base(message, innerException) { }

    protected AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
