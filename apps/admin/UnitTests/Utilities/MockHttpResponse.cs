namespace UnitTests.Utilities;

internal class MockHttpResponse
{
    public MockHttpResponse(string message) => Message = message;

    public string Message { get; init; }
}
