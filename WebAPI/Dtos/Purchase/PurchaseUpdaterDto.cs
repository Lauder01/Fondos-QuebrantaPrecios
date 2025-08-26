using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseUpdaterDto : PurchaseBaseDto
    {
        [Required(ErrorMessage = "ERR011: El campo Identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR012: El campo Identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
