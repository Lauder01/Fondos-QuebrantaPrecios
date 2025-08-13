using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentUpdaterDto : ApartmentBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
