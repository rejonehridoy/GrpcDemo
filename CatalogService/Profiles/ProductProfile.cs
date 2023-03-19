using AutoMapper;
using CatalogService.Models;

namespace CatalogService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Source ->  Destination
            CreateMap<Product, GrpcProductModel>();
        }
    }
}
