using AutoMapper;
using ShoppingCartService.Models;

namespace ShoppingCartService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GrpcProductModel, Product>();
        }
    }
}
