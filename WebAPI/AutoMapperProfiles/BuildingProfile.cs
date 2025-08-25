using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Building;

namespace WebAPI.AutoMapperProfiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingGetterDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<BuildingCreatorDto, Building>();
            CreateMap<BuildingUpdaterDto, Building>();
        }
    }
}
