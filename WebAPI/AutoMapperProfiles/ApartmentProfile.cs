using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.Apartment;
using WebAPI.Dtos.CozyHouse;

namespace WebAPI.AutoMapperProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<Apartment, ApartmentGetterDto>();
            CreateMap<ApartmentCreatorDto, Apartment>();
            CreateMap<ApartmentUpdaterDto, Apartment>();
            CreateMap<Apartment, CozyHouseCreatorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Door, opt => opt.MapFrom(src => src.Door))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor != null ? src.Floor.FloorNumber : 0))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Floor != null && src.Floor.Building != null && src.Floor.Building.Price.HasValue ? src.Floor.Building.Price.Value : 0))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.NumberOfRooms, opt => opt.MapFrom(src => src.NumRooms))
                .ForMember(dest => dest.NumberOfBathrooms, opt => opt.MapFrom(src => src.NumBathrooms))
                .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.Floor != null && src.Floor.Building != null ? src.Floor.Building.Id : string.Empty))
                .ForMember(dest => dest.BuildingCode, opt => opt.MapFrom(src => src.Floor != null && src.Floor.Building != null ? src.Floor.Building.Code : string.Empty))
                .ForMember(dest => dest.HasLift, opt => opt.MapFrom(src => src.Floor != null && src.Floor.Building != null ? src.Floor.Building.HasElevator : false))
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => src.Floor != null && src.Floor.Building != null ? src.Floor.Building.HasGarage : false));
        }
    }
}
