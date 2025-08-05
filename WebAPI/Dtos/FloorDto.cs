using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class FloorDto
    {
        public Guid Id { get; set; }
        public int FloorNumber { get; set; }
        public bool HasLift { get; set; }
        public Guid BuildingId { get; set; }
    }
    public class CreateFloorDto
    {
        [Required]
        [Range(-12, 100)] // FloorNumber >= -12
        public int FloorNumber { get; set; }
        [Required]
        public bool HasLift { get; set; }
        [Required]
        public Guid BuildingId { get; set; }
    }
}
