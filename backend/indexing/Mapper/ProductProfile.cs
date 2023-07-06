using Application.Models;
using AutoMapper;
using Domain;
using System.Diagnostics.CodeAnalysis;

namespace Mapper;

[ExcludeFromCodeCoverage]
public class ProductProfile : Profile
{
    public ProductProfile() 
    {
        CreateMap<Product, IndexableProduct>();
    }
}
