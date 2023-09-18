namespace UserManagement.Models;

internal record struct UserRolesResponse(string Status, IEnumerable<string> Users);