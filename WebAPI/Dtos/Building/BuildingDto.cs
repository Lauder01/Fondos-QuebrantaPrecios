using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Doorway { get; set; } = string.Empty;
        public int? FloorCount { get; set; }
        public int? YearBuilt { get; set; }
        public double? Price { get; set; }
        public string? DistrictName { get; set; }
        public string? StreetName { get; set; }
    }
    public class CreateBuildingDto
    {
        [StringLength(255)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 1)]
        public string Doorway { get; set; } = string.Empty;
        [Range(0, 100)]
        public int? FloorCount { get; set; }
        [Range(1800, 2100)]
        public int? YearBuilt { get; set; }
        [Range(0, double.MaxValue)]
        public double? Price { get; set; }
        [Required]
        public Guid DistrictId { get; set; }
        [Required]
        public Guid StreetId { get; set; }
    }
}
