using System.Text.Json.Serialization;

namespace Authentication.Models
{
    internal class RolesClaimModel
    {
        [JsonPropertyName("t")]
        public required long AssignedAt { get; init; }
        [JsonPropertyName("v")]
        public required IEnumerable<string> Roles { get; init; }
    }

}
