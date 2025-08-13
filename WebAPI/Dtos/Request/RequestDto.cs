namespace WebAPI.Dtos.Request
{
    public class RequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string BuildingId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public float MaintenancePrice { get; set; }
        public string StatusId { get; set; } = string.Empty;
        // Puedes agregar más propiedades según lo que quieras exponer
    }
}
