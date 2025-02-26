namespace UserManagement.Models;

internal record UserRolesResponse(string Status, IEnumerable<string> Users);