using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Floor;

namespace WebAPI.AutoMapperProfiles
{
    public class FloorProfile : Profile
    {
        public FloorProfile()
        {
            CreateMap<Floor, FloorGetterDto>();
            CreateMap<FloorCreatorDto, Floor>();
            CreateMap<FloorUpdaterDto, Floor>();
        }
    }
}
