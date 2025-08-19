using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.District;
using System.Linq;

namespace WebAPI.AutoMapperProfiles
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictGetterDto>()
                .ForMember(dest => dest.Zipcodes, opt => opt.MapFrom(src => src.Zipcode != null ? src.Zipcode.Select(z => z.Code).ToList() : new List<string>()));
            CreateMap<DistrictGetterDto, District>();
        }
    }
}