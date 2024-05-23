using Application.Exceptions;
using Application.UserManagement;
using Domain;
using System.Text.Json;
using UserManagement.Models;

namespace UserManagement;

internal class UserClient(HttpClient httpClient) : IUserClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    public async IAsyncEnumerable<User> GetUsersAsync()
    {
        var userManagementResponse = await GetUserDataAsync();
        var userRoles = await GetUsersInAdminRoleAsync();
        var adminUsers = userRoles.Users.ToList();
        foreach(var userData in userManagementResponse.Users)
            yield return new(userData.User.Email, adminUsers.Contains(userData.User.Id), DateTimeOffset.FromUnixTimeMilliseconds(userData.User.TimeJoined).UtcDateTime);
    }

    private async Task<GetUsersResponse> GetUserDataAsync()
    {
        var response = await _httpClient.GetAsync("/users");
        var responseContent = await response.Content.ReadAsStringAsync();
        var usersResponse = JsonSerializer.Deserialize<GetUsersResponse>(responseContent, _serializerOptions);
        return !usersResponse.Equals(default) ? usersResponse : throw new MicroshopApiException("Unable to retrieve users data");
    }

    private async Task<UserRolesResponse> GetUsersInAdminRoleAsync()
    {
        var response = await _httpClient.GetAsync("/recipe/role/users?role=admin");
        var responseContent = await response.Content.ReadAsStringAsync();
        var userRolesResponse = JsonSerializer.Deserialize<UserRolesResponse>(responseContent, _serializerOptions);
        return !userRolesResponse.Equals(default) ? userRolesResponse : throw new MicroshopApiException("Unable to retrieve role data");
    }
}
