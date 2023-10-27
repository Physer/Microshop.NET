using System.Net;
using UnitTests.Utilities;
using UserManagement;

namespace UnitTests.UserManagement;

internal class UserClientBuilder : HttpClientBuilder<UserClientBuilder>
{
    private readonly HashSet<FakeHttpMessage> _fakeHttpMessages;

    public UserClientBuilder() => _fakeHttpMessages = new();

    public UserClientBuilder WithGetUserDataAsyncReturning(object getUsersResponse)
    {
        FakeHttpMessage httpMessage = new(HttpStatusCode.OK, getUsersResponse, Array.Empty<KeyValuePair<string, string>>(), "/users");
        _fakeHttpMessages.Add(httpMessage);

        return this;
    }

    public UserClientBuilder WithGetUsersInAdminRoleAsyncReturning(object userRolesResponse)
    {
        FakeHttpMessage httpMessage = new(HttpStatusCode.OK, userRolesResponse, Array.Empty<KeyValuePair<string, string>>(), "/recipe/role/users?role=admin");
        _fakeHttpMessages.Add(httpMessage);

        return this;
    }

    public UserClient Build() => new(BuildHttpClient(_fakeHttpMessages));
}
