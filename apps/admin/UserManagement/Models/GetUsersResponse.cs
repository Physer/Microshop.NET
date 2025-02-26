namespace UserManagement.Models;

internal record GetUsersResponse(string Status, IEnumerable<UsersResponse> Users);
