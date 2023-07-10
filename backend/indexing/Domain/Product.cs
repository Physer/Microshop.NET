namespace Domain;

public record Product
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
