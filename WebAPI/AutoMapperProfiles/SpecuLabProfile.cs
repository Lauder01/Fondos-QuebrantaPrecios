using AutoMapper;
using ClassLibraryProject.Entities;
using WebAPI.Dtos.SpecuLab;
using System.Linq;

namespace WebAPI.AutoMapperProfiles
{
    public class SpecuLabProfile : Profile
    {
        public SpecuLabProfile()
        {
            CreateMap<SpecuLabCreatorDto, Building>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.BuildingCode))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.BuildingAmount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Building, SpecuLabCreatorDto>()
                .ForMember(dest => dest.BuildingCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Code) ? "SIN-CODIGO" : src.Code.Trim()))
                .ForMember(dest => dest.BuildingAmount, opt => opt.MapFrom(src => (src.Price == null || src.Price <= 0) ? "0" : src.Price.ToString()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Description) ? "Edificio sin descripción" : src.Description.Trim()))
                .ForMember(dest => dest.MaintenanceAmount, opt => opt.MapFrom(src => 0));
            CreateMap<Building, SpecuLabGetterDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ConstructedAddress, opt => opt.MapFrom(src => src.Address.FirstOrDefault() != null ? src.Address.FirstOrDefault().ConstructedAddress : string.Empty))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : string.Empty))
                .ForMember(dest => dest.FloorCount, opt => opt.MapFrom(src => src.FloorCount))
                .ForMember(dest => dest.YearBuilt, opt => opt.MapFrom(src => src.YearBuilt))
                .ForMember(dest => dest.ApartmentCount, opt => opt.MapFrom(src => src.ApartmentCount));
        }
    }
}