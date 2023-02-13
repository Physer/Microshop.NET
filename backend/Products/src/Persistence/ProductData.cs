namespace Persistence;

internal class ProductData
{
    public ProductData()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; init; }
    public required string ProductCode { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
