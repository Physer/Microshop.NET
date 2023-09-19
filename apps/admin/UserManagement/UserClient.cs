using Application.UserManagement;
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
        var userManagementResponse = await GetUserData();
        var userRoles = await GetUsersInAdminRole();
        var adminUsers = userRoles.Users.ToList();
        foreach(var userData in userManagementResponse.Users)
            yield return new(userData.User.Email, adminUsers.Contains(userData.User.Id), DateTimeOffset.FromUnixTimeMilliseconds(userData.User.TimeJoined).UtcDateTime);
    }

    private async Task<GetUsersResponse> GetUserData()
    {
        var response = await _httpClient.GetAsync("/users");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GetUsersResponse>(responseContent, _serializerOptions);
    }

    private async Task<UserRolesResponse> GetUsersInAdminRole()
    {
        var response = await _httpClient.GetAsync("/recipe/role/users?role=admin");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserRolesResponse>(responseContent, _serializerOptions);
    }
}
