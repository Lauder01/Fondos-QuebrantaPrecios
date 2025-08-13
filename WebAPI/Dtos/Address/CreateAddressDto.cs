namespace WebAPI.Dtos.Address
{
    public class CreateAddressDto
    {
        public string? BuildingId { get; set; }
        public string? ApartmentId { get; set; }
        public bool? IsApartment { get; set; }
        // Puedes agregar más propiedades según lo que quieras recibir
    }
}
