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
        CreateMap<Product, IndexableProduct>().ForMember(indexedProduct => indexedProduct.Id, source => source.MapFrom(product => product.Code));
    }
}
