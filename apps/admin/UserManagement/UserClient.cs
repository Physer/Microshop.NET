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

    private async IAsyncEnumerable<UsersResponse> GetUserData()
    {
        var response = await _httpClient.GetAsync("/users");
        var responseContent = await response.Content.ReadAsStringAsync();
        var usersResponse = JsonSerializer.Deserialize<UsersResponse>(responseContent, _serializerOptions);
        yield return usersResponse;
    }

    private async IAsyncEnumerable<UserRolesResponse> GetUsersInAdminRole()
    {
        yield break;
    }
}
