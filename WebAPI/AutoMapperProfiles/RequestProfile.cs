using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Request;

namespace WebAPI.AutoMapperProfiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<Request, RequestGetterDto>();
            CreateMap<RequestCreatorDto, Request>();
            CreateMap<RequestUpdaterDto, Request>();
        }
    }
}
