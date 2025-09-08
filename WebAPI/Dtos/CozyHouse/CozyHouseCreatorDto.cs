using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.CozyHouse
{
    public class CozyHouseCreatorDto
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Door { get; set; } = string.Empty;
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string BuildingId { get; set; } = string.Empty;
        public string BuildingCode { get; set; } = string.Empty;
        public bool HasLift { get; set; }
        public bool HasGarage { get; set; }
    }
}
