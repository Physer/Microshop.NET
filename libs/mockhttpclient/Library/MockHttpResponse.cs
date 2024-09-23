namespace Microshop.Library;

/// <summary>
/// A fake HTTP response object that can be used as a return type for your HTTP requests
/// </summary>
/// <remarks>
/// Create an object with a Message property
/// </remarks>
/// <param name="message">The value of the Message property</param>
public class MockHttpResponse(string message)
{
    public string Message { get; init; } = message;
}
