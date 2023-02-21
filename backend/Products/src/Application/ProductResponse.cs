namespace Application;

public record ProductResponse
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
