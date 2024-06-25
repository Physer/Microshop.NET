using System.Collections;

namespace Tests.Data;

internal class InvalidFakeHttpMessageHeaderValues : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("", [""]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("", ["value", "value2"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new(" ", [" "]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("", [" "]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new(" ", [""]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("Content-Type", ["application/json"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("0MgWhat1sThis?!", ["asdfhaksd78y23iukjazs~^6t"]) } };
        yield return new object[] { new List<KeyValuePair<string, IEnumerable<string>>> { new("", [""]), new(" ", [" "]), new("", [" "]), new(" ", [""]) } };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
