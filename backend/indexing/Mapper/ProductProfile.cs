using Application.Models;
using AutoMapper;
using Domain;

namespace Mapper;

public class ProductProfile : Profile
{
    public ProductProfile() 
    {
        CreateMap<Product, IndexableProduct>();
    }
}
