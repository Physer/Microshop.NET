using Application.Models;
using AutoMapper;
using Domain;
using System.Diagnostics.CodeAnalysis;

namespace Mapper;

[ExcludeFromCodeCoverage]
public class IndexingProfile : Profile
{
    public IndexingProfile()
    {
        CreateMap<Product, IndexableProduct>().ForMember(indexedProduct => indexedProduct.Id, source => source.MapFrom(product => product.Code));
        CreateMap<Price, IndexableProduct>().ForMember(indexedProduct => indexedProduct.Id, source => source.MapFrom(price => price.ProductCode));
    }
}
