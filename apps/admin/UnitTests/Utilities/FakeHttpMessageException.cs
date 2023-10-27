using System.Runtime.Serialization;

namespace UnitTests.Utilities;

/// <summary>
/// An exception to indicate it's impossible to mock the specified HTTP call with the fake HTTP message handler
/// </summary>
internal class FakeHttpMessageException : Exception
{
    public FakeHttpMessageException() { }

    public FakeHttpMessageException(string? message) : base(message) { }

    public FakeHttpMessageException(string? message, Exception? innerException) : base(message, innerException) { }

    protected FakeHttpMessageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
