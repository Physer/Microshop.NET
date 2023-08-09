using Domain;

namespace Application.Interfaces.Generator;

public interface IPriceGenerator
{
    IEnumerable<Price> GeneratePrices(IEnumerable<Product>? productData);
}
