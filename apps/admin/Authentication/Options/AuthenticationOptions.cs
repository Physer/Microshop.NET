namespace Authentication.Options;

internal sealed class AuthenticationOptions
{
    public const string ConfigurationEntry = "Authentication";

    public required string BaseUrl { get; init; }
}
