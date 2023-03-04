namespace Application.Models;

public record IndexableProduct : ProductResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
