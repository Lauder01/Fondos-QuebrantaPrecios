using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseUpdaterDto : PurchaseBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
    }
}
