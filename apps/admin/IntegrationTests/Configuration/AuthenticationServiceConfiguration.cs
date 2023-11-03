using Microshop.ContainerConfiguration;

namespace IntegrationTests.Configuration;

internal class AuthenticationServiceConfiguration : IContainerConfiguration
{
    public string ImageName => "physer/microshop-authentication:main";

    public int? Port => 5004;

    public required string AuthenticationCoreIpAddress { get; init; }
    public required int AuthenticationCorePort { get; init; }
    public static string Hostname => "localhost";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "AUTHENTICATION_CORE_URL", $"http://{AuthenticationCoreIpAddress}:{AuthenticationCorePort}" },
        { "AUTHENTICATION_BACKEND_HOST", Hostname },
        { "AUTHENTICATION_BACKEND_PORT", $"{Port}" },
        { "GATEWAY_URL", $"http://localhost:5000" },
        { "WEBSITE_URL", "http://localhost:3000" },
        { "DASHBOARD_USER_EMAIL", Constants.DefaultTextValue },
        { "DASHBOARD_USER_PASSWORD", Constants.DefaultTextValue },
        { "ADMIN_KEY", Constants.DefaultTextValue }
    };
}
