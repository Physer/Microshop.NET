namespace Application.Options;

public class AuthenticationOptions
{
    public const string ConfigurationEntry = "Authentication";

    public required string BaseUrl { get; init; }
    public required string Issuer { get; init; }
    public required string RelativeJwksEndpoint { get; init; }
}
