using System.Collections.Immutable;

namespace Microshop.ContainerConfiguration.ContainerConfigurations;

public sealed class ServicebusContainerConfiguration : IContainerConfiguration
{
    public static string Hostname => "localhost";
    public static string Username => "guest";
    public static string Password => "guest";
    public int? Port => 5672;

    public string ImageName => "masstransit/rabbitmq";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => ImmutableDictionary<string, string>.Empty;
}
