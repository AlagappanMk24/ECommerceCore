using AutoMapper;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Mappings
{
    // CategoryProfile.cs
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}
