namespace Microshop.ContainerConfiguration
{
    internal sealed class MicroshopApiConfiguration : IContainerConfiguration
    {
        public string ImageName => "physer/microshop-api";

        public int? Port => 80;

        public required string RabbitMqContainerIp { get; init; }
        public required string RabbitMqUsername { get; init; }
        public required string RabbitMqPassword { get; init; }
        public required string AuthenticationServiceContainerIp { get; init; }

        public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
        {
            {"Servicebus__BaseUrl", RabbitMqContainerIp },
            {"Servicebus__ManagementUsername", RabbitMqContainerIp },
            {"Servicebus__ManagementPassword", RabbitMqContainerIp },
            {"Authentication__BaseUrl",  $"http://{AuthenticationServiceContainerIp}" },
            {"Authentication__Issuer", "http://localhost:5000/auth" },
        };
    }
}
