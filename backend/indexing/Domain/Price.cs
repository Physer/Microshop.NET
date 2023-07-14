namespace Domain;

public record Price
{
    public required string ProductCode { get; init; }
    public required decimal Value { get; init; }
    public required string Currency { get; init; }
}
