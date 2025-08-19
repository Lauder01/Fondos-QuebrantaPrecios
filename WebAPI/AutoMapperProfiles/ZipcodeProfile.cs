using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Zipcode;

namespace WebAPI.AutoMapperProfiles
{
    public class ZipcodeProfile : Profile
    {
        public ZipcodeProfile()
        {
            CreateMap<Zipcode, ZipcodeGetterDto>();
            CreateMap<ZipcodeGetterDto, Zipcode>();
        }
    }
}
