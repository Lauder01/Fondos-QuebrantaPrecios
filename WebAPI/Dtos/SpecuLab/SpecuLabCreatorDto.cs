using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    public class SpecuLabCreatorDto
    {
        [Required]
        public string BuildingCode { get; set; }
        [Required]
        public string BuildingName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public int FloorCount { get; set; }
        [Required]
        public int YearBuilt { get; set; }
        // public string ApartmentNumber { get; set; } // Para uso futuro
    }
}