using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentUpdaterDto : ApartmentBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
    }
}
