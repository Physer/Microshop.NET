using Application;
using AutoMapper;
using Domain;
using Persistence;

namespace Mapper;

public class ProductProfile : Profile
{
    public ProductProfile() 
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, ProductData>();
        CreateMap<ProductData, Product>();
    }
}
