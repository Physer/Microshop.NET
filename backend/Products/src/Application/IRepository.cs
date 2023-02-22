﻿using Domain;

namespace Application;

public interface IRepository
{
    IEnumerable<Product> GetProducts();
    Product? GetProductById(Guid id);
    void CreateProduct(Product product);
    void CreateProducts(IEnumerable<Product> products);
}
