using AutoMapper;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ItemCategory.Category));
            CreateMap<Item, ItemResponseDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ItemCategory.Category));
        }
    }
}
