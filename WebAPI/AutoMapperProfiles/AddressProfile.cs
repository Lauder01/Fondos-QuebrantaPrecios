using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Address;

namespace WebAPI.AutoMapperProfiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressGetterDto>();
            CreateMap<AddressCreatorDto, Address>()
                .ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.ApartmentId) ? null : src.ApartmentId)); // Convertir string.Empty a null
            CreateMap<AddressUpdaterDto, Address>()
                .ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.ApartmentId) ? null : src.ApartmentId)); // Convertir string.Empty a null
        }
    }
}
