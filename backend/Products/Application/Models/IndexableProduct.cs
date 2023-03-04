namespace Application.Models;

public record IndexableProduct : ProductResponse
{
    public static Guid Id => Guid.NewGuid();
}
