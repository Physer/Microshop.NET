namespace UserManagement.Models;

internal record struct GetUsersResponse(string Status, IEnumerable<UsersResponse> Users);
