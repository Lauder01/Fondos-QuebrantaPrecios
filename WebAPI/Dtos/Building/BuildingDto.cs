namespace WebAPI.Dtos.Building
{
    public class BuildingDto : BuildingBaseDto
    {
        // Code es el identificador principal, ya está en la base
        public string? DistrictName { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingCompanyName { get; set; }
    }
}
