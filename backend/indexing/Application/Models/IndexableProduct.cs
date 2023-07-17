namespace Application.Models;

public record struct IndexableProduct(string Id)
{
    public required string? Name { get; init; }
    public required string? Description { get; init; }
    public required decimal? Value { get; init; }
    public required string? Currency { get; init; }
}
