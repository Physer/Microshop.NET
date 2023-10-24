using System.Text.Json.Serialization;

namespace Authentication.Models;

internal class MicroshopRoleClaim
{
    [JsonPropertyName("t")]
    public required long AcquiredAt { get; init; }
    [JsonPropertyName("v")]
    public required IEnumerable<string> Roles { get; init; }
}
