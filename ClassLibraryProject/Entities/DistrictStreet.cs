using System.ComponentModel.DataAnnotations;

namespace ClassLibraryProject.Entities
{
    public class DistrictStreet
    {
        [Required]
        public string DistrictId { get; set; } = string.Empty;
        [Required]
        public string StreetId { get; set; } = string.Empty;
        // Relaciones de navegación opcionales
        public virtual District? District { get; set; }
        public virtual Street? Street { get; set; }
    }
}
