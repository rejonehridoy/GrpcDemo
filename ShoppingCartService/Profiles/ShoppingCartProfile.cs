using AutoMapper;
using ShoppingCartService.Dtos;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartService.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            // Source -> Target
            CreateMap<ShoppingCartItemCreateDto, ShoppingCartItem>();
            CreateMap<ShoppingCartItem, ShoppingCartItemReadDto>();

            //CreateMap<GrpcPlatformModel, Platform>()
            //    .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}
