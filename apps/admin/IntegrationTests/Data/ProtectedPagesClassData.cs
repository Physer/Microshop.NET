using System.Collections;

namespace IntegrationTests.Data;

internal class ProtectedPagesClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[] { "/" };
        yield return new[] { "/Index" };
        yield return new[] { "/ManageData" };
        yield return new[] { "/ManageUsers" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
