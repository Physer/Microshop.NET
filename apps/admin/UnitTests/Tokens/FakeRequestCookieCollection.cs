using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Tokens;

internal class FakeRequestCookieCollection(string cookieName, string cookieValue) : IRequestCookieCollection
{
    private readonly string _cookieName = cookieName;
    private readonly string _cookieValue = cookieValue;

    public string? this[string key] => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

    public ICollection<string> Keys => throw new NotImplementedException();

    public bool ContainsKey(string key) => throw new NotImplementedException();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => throw new NotImplementedException();

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        if (key.Equals(_cookieName))
        {
            value = _cookieValue;
            return true;
        }

        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
