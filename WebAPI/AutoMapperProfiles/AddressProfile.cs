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
            CreateMap<AddressCreatorDto, Address>();
            CreateMap<AddressUpdaterDto, Address>();
        }
    }
}
