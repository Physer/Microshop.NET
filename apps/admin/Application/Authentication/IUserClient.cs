using Domain;

namespace Application.Authentication;

public interface IUserClient
{
    IAsyncEnumerable<User> GetUsers();
}
