using UnitTests.Utilities;
using UserManagement;

namespace UnitTests.UserManagement;

internal class UserClientBuilder : HttpClientBuilder<UserClientBuilder>
{
    public UserClient Build() => new(BuildHttpClient());
}
