using System.Collections;

namespace IntegrationTests.Data;

internal class IndexUrlsClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[] { "/" };
        yield return new[] { "/index" };
        yield return new[] { "/Index" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
