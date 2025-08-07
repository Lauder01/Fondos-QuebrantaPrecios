namespace WebAPI.Dtos
{
    public class AddressDto
    {
        public string Id { get; set; } = string.Empty;
        public string? BuildingId { get; set; }
        public string? ApartmentId { get; set; }
        public bool? IsApartment { get; set; }
        // Puedes agregar más propiedades según lo que quieras exponer
    }
}
