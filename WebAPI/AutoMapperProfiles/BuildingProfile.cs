using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Building;

namespace WebAPI.AutoMapperProfiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingGetterDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.ConstructedAddress, opt => opt.MapFrom(src => GetAddressField(src, a => a.ConstructedAddress)))
                .ForMember(dest => dest.ZipcodeId, opt => opt.MapFrom(src => GetAddressField(src, a => a.ZipcodeId)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => GetAddressField(src, a => a.Country)))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => GetAddressField(src, a => a.City)))
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => src.HasGarage));
            CreateMap<BuildingCreatorDto, Building>()
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => src.HasGarage));
            CreateMap<BuildingWithImagesCreatorDto, Building>()
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => src.HasGarage));
            CreateMap<BuildingUpdaterDto, Building>()
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => src.HasGarage));
        }

        private static string? GetConstructedAddress(Building src)
        {
            if (src.Address == null) return null;
            var address = src.Address.FirstOrDefault(a => a.ApartmentId == null);
            return address?.ConstructedAddress;
        }

        private static string? GetAddressField(Building src, Func<ClassLibraryProject.Entities.Address, string?> selector)
        {
            if (src.Address == null) return null;
            var address = src.Address.FirstOrDefault(a => a.ApartmentId == null);
            return address != null ? selector(address) : null;
        }
    }
}
