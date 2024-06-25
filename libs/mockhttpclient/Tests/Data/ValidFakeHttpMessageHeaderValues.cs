using System.Collections;

namespace Tests.Data;

internal class ValidFakeHttpMessageHeaderValues : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip", ""]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip", " "]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip", "", " "]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip", "xml", "json"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Bogus", ["foo"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip"]), new("Accept-Language", ["en-US"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Accept", ["gzip"]), new("Accept-Language", ["en-US"]), new("Bogus", ["foo"]) } };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
