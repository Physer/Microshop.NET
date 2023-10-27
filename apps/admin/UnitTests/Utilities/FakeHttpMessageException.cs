using System.Runtime.Serialization;

namespace UnitTests.Utilities;

internal class FakeHttpMessageException : Exception
{
    public FakeHttpMessageException() { }

    public FakeHttpMessageException(string? message) : base(message) { }

    public FakeHttpMessageException(string? message, Exception? innerException) : base(message, innerException) { }

    protected FakeHttpMessageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
