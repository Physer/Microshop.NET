using System.Collections.Immutable;

namespace IntegrationTests.Configuration;

internal class RabbitMqContainerConfiguration : IContainerConfiguration
{
    public string Username => "guest";
    public string Password => "guest";

    public string ImageName => "masstransit/rabbitmq";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => ImmutableDictionary<string, string>.Empty;
}
