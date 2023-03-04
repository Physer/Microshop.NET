using Domain;

namespace Application.Interfaces.Generator;

public interface IProductGenerator
{
    IEnumerable<Product> GenerateProducts(int amountToGenerate);
}
