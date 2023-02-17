namespace Persistence;

internal class ProductData
{
    public required Guid Id { get; init; }
    public required string ProductCode { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
