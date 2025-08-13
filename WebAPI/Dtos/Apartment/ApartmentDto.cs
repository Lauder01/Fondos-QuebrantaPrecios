using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Door { get; set; } = string.Empty;
        public Guid FloorId { get; set; }
        public Guid BuildingId { get; set; }
    }
    public class CreateApartmentDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [StringLength(24, MinimumLength = 1)]
        public string Door { get; set; } = string.Empty;
        [Required]
        public Guid FloorId { get; set; }
        [Required]
        public Guid BuildingId { get; set; }
    }
}
