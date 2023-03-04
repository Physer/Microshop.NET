using Domain;

namespace Application.Models;

public class IndexableProduct : Product
{
    public static Guid Id => Guid.NewGuid();
}
