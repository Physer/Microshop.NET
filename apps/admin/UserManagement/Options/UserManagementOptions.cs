namespace UserManagement.Options;

internal sealed class UserManagementOptions
{
    public const string ConfigurationEntry = "UserManagement";

    public required string BaseUrl { get; init; }
}
