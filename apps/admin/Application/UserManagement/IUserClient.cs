using Domain;

namespace Application.UserManagement;

public interface IUserClient
{
    IAsyncEnumerable<User> GetUsersAsync();
}
