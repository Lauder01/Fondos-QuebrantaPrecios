using System.ComponentModel.DataAnnotations;
/// <summary>
/// Este dto está para que speculab acceda a la request de un edificio en concreto.
/// </summary> 

namespace WebAPI.Dtos.SpecuLab
{
    public class SpecuLabGetterDto
    {
        public string BuildingName { get; set; }
        public string ConstructedAddress { get; set; }
        public string DistrictName { get; set; }
        public int FloorCount { get; set; }
        public int YearBuilt { get; set; }
        public int ApartmentCount { get; set; }
    }
}