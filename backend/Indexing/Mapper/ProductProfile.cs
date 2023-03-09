using Application.Models;
using AutoMapper;
using Domain;
using ProductsClient.Contracts;

namespace Mapper;

public class ProductProfile : Profile
{
    public ProductProfile() 
    {
        CreateMap<Product, IndexableProduct>();
        CreateMap<ProductResponse, Product>();
    }
}
