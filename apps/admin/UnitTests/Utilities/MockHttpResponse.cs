namespace UnitTests.Utilities;

/// <summary>
/// A fake HTTP response object that can be used as a return type for your HTTP requests
/// </summary>
internal class MockHttpResponse
{
    /// <summary>
    /// Create an object with a Message property
    /// </summary>
    /// <param name="message">The value of the Message property</param>
    public MockHttpResponse(string message) => Message = message;

    public string Message { get; init; }
}
