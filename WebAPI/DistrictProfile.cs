using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos;

public class DistrictProfile : Profile
{
    public DistrictProfile()
    {
        CreateMap<District, DistrictDto>()
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Zipcode));
        CreateMap<CreateDistrictDto, District>()
            .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.ZipCode));
    }
}
