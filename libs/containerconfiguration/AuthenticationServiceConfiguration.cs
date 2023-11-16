namespace Microshop.ContainerConfiguration
{
    internal sealed class AuthenticationServiceConfiguration : IContainerConfiguration
    {
        public string ImageName => "physer/microshop-authentication";

        public int? Port => 80;

        public required string SupertokensContainerIpAddress { get; init; }
        public required int SupertokensContainerPort { get; init; }
        public static string Hostname => "localhost";
        public static string AdminKey => "admin_key";

        public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
        {
            { "AUTHENTICATION_CORE_URL", $"http://{SupertokensContainerIpAddress}:{SupertokensContainerPort}" },
            { "AUTHENTICATION_BACKEND_HOST", "localhost" },
            { "AUTHENTICATION_BACKEND_PORT", "80" },
            { "GATEWAY_URL", "http://localhost" },
            { "WEBSITE_URL", "http://localhost" },
            { "ADMIN_KEY", AdminKey },
        };
    }
}
