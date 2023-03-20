using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;

namespace CatalogService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Source ->  Destination
            CreateMap<Product, GrpcProductModel>();
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
