using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Apartment;

namespace WebAPI.AutoMapperProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<Apartment, ApartmentGetterDto>();
            CreateMap<ApartmentCreatorDto, Apartment>();
            CreateMap<ApartmentUpdaterDto, Apartment>();
        }
    }
}
