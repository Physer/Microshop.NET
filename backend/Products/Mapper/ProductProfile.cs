using AutoMapper;
using Domain;
using Persistence;

namespace Mapper;

public class ProductProfile : Profile
{
    public ProductProfile() 
    {
        CreateMap<Product, ProductData>();
        CreateMap<ProductData, Product>();
    }
}
