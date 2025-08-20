using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Address;

namespace WebAPI.AutoMapperProfiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressGetterDto>()
                .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.ZipcodeId))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.StreetId));
            CreateMap<AddressCreatorDto, Address>();
            CreateMap<AddressUpdaterDto, Address>();
        }
    }
}
