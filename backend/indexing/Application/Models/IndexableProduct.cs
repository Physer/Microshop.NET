namespace Application.Models;

public record struct IndexableProduct(string Id)
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal? Value { get; init; }
    public string? Currency { get; init; }
}
