using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Building;

namespace WebAPI.AutoMapperProfiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingGetterDto>();
            CreateMap<BuildingCreatorDto, Building>();
            CreateMap<BuildingUpdaterDto, Building>();
        }
    }
}
