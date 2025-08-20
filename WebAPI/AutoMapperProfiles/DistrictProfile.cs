using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.District;
using System.Linq;
using System;

namespace WebAPI.AutoMapperProfiles
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictGetterDto>()
                .ForMember(dest => dest.Zipcodes, opt => opt.MapFrom(src => 
                    src.Zipcode != null && src.Zipcode.Any() 
                        ? src.Zipcode.Select(z => z.Code).ToList() 
                        : new List<string>()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
                
            CreateMap<DistrictGetterDto, District>();
        }
    }
}