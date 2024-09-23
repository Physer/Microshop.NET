using System.Collections;
using System.Net;

namespace Microshop.Library;

public class InvalidHttpResponseClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { HttpStatusCode.BadGateway };
        yield return new object[] { HttpStatusCode.BadRequest };
        yield return new object[] { HttpStatusCode.Forbidden };
        yield return new object[] { HttpStatusCode.Unauthorized };
        yield return new object[] { HttpStatusCode.InternalServerError };
        yield return new object[] { HttpStatusCode.NotFound };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
