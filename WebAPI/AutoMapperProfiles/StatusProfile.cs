using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Status;

namespace WebAPI.AutoMapperProfiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<Status, StatusBaseDto>();
            CreateMap<Status, StatusGetterDto>();
        }
    }
}
