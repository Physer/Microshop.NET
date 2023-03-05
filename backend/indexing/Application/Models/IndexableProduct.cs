using Domain;

namespace Application.Models;

public record IndexableProduct : Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
