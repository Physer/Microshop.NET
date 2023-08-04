namespace Application.Options;

public class ProxyOptions
{
    public const string ConfigurationEntry = "ReverseProxy";
    public required ProxyClusterElement Clusters { get; init; }
}

public class ProxyClusterElement
{
    public required ProxyClusterDestinationElement Authentication { get; init; }
}

public class ProxyClusterDestinationElement
{
    public required ProxyClusterDestination Destinations { get; init; }
}

public class ProxyClusterDestination
{
    public required ProxyClusterDestinationValue Authentication { get; init; }
}

public class ProxyClusterDestinationValue
{
    public required string Address { get; init; }
}