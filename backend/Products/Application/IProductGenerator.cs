using Domain;

namespace Application;

public interface IProductGenerator
{
    IEnumerable<Product> GenerateProducts(int amountToGenerate);
}
