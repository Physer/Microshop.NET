using Application.Models;
using AutoMapper;
using Domain;
using Persistence;
using ProductsClient.Contracts;

namespace Mapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, ProductData>();
        CreateMap<ProductData, Product>();
        CreateMap<Product, GetProductsResponse>();
    }
}