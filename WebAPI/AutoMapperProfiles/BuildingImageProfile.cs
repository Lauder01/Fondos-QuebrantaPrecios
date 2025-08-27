using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.BuildingImage;

namespace WebAPI.AutoMapperProfiles
{
    public class BuildingImageProfile : Profile
    {
        public BuildingImageProfile()
        {
            CreateMap<BuildingImage, BuildingImageGetterDto>();
            CreateMap<BuildingImageCreatorDto, BuildingImage>();
            CreateMap<BuildingImageUpdaterDto, BuildingImage>();
        }
    }
}