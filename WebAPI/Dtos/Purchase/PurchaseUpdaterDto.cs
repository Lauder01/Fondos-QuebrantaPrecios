using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseUpdaterDto : PurchaseBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
