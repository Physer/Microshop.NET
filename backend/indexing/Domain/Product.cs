namespace Domain;

public record Product
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
