using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Street;

namespace WebAPI.AutoMapperProfiles
{
    public class StreetProfile : Profile
    {
        public StreetProfile()
        {
            CreateMap<Street, StreetGetterDto>();
            CreateMap<StreetCreatorDto, Street>();
            CreateMap<StreetUpdaterDto, Street>();
        }
    }
}
