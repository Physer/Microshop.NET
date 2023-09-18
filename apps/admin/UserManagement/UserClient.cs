using Application.Authentication;
using Domain;
using System.Text.Json;
using UserManagement.Models;

namespace UserManagement;

internal class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public UserClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async IAsyncEnumerable<User> GetUsers()
    {
        yield break;
    }

    public async IAsyncEnumerable<UsersResponse> GetUserData()
    {
        yield break;
    }

    public async IAsyncEnumerable<UserRolesResponse> GetUsersInAdminRole()
    {
        yield break;
    }
}
