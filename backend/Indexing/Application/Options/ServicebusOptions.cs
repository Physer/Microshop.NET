namespace Application.Options;

public class ServicebusOptions
{
    public const string ConfigurationEntry = "Servicebus";

    public string? BaseUrl { get; set; }
    public string? ManagementUsername { get; set; }
    public string? ManagementPassword { get; set; }
}
