using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.District;

namespace WebAPI.AutoMapperProfiles
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictGetterDto>()
                .ForMember(dest => dest.ZipCodes, opt => opt.MapFrom(src => src.GetZipCodes()));
            CreateMap<DistrictGetterDto, District>();
        }
    }
}