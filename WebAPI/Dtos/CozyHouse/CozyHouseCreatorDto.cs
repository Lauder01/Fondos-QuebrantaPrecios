using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.CozyHouse
{
    public class CozyHouseCreatorDto
    {
        [Required]
        [StringLength(36, MinimumLength = 1, ErrorMessage = "El Id debe tener entre 1 y 36 caracteres")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El código debe tener entre 1 y 50 caracteres")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "La puerta debe tener entre 1 y 10 caracteres")]
        public string Door { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "El piso debe estar entre 0 y 100")]
        public int Floor { get; set; }

        [Range(0.0, 9999999999.99, ErrorMessage = "El precio debe estar entre 0 y 9999999999.99")]
        public decimal Price { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "El área debe estar entre 0 y 10000")]
        public decimal Area { get; set; }

        [Range(0, 100, ErrorMessage = "El número de habitaciones debe estar entre 0 y 100")]
        public int NumberOfRooms { get; set; }

        [Range(0, 50, ErrorMessage = "El número de baños debe estar entre 0 y 50")]
        public int NumberOfBathrooms { get; set; }

        [Required]
        [StringLength(36, MinimumLength = 1, ErrorMessage = "El BuildingId debe tener entre 1 y 36 caracteres")]
        public string BuildingId { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El BuildingCode debe tener entre 1 y 50 caracteres")]
        public string BuildingCode { get; set; } = string.Empty;

        public bool HasLift { get; set; }
        public bool HasGarage { get; set; }
    }
}
