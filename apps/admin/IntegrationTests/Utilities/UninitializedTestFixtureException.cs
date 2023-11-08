using System.Runtime.Serialization;

namespace IntegrationTests.Utilities;

/// <summary>
/// An exception to be thrown by integration tests when the Fixture has not been initialized
/// </summary>
internal class UninitializedTestFixtureException : Exception
{
    /// <summary>
    /// The default constructor creates an exception with a generic fixture failure message
    /// </summary>
    public UninitializedTestFixtureException() : this("Test Fixture has not been initialized") { }

    public UninitializedTestFixtureException(string? message) : base(message) { }

    public UninitializedTestFixtureException(string? message, Exception? innerException) : base(message, innerException) { }

    protected UninitializedTestFixtureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
