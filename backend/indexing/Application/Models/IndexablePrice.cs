using Domain;

namespace Application.Models;

public record IndexablePrice : Price
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
