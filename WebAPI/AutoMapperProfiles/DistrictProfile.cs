using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.District;

namespace WebAPI.AutoMapperProfiles
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictGetterDto>();
            CreateMap<DistrictGetterDto, District>();
        }
    }
}