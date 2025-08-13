using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.BuildingCompany;

namespace WebAPI.AutoMapperProfiles
{
    public class BuildingCompanyProfile : Profile
    {
        public BuildingCompanyProfile()
        {
            CreateMap<BuildingCompany, BuildingCompanyGetterDto>();
            CreateMap<BuildingCompanyCreatorDto, BuildingCompany>();
            CreateMap<BuildingCompanyUpdaterDto, BuildingCompany>();
        }
    }
}
