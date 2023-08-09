using System.Collections.Immutable;

namespace IntegrationTests.Configuration;

internal class RabbitMqContainerConfiguration : IContainerConfiguration
{
    public static string Hostname => "localhost";
    public static string Username => "guest";
    public static string Password => "guest";
    public static int Port => 5672;

    public string ImageName => "masstransit/rabbitmq";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => ImmutableDictionary<string, string>.Empty;
}
