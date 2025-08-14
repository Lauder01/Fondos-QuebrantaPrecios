namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        // Dirección completa generada desde Address.ToString()
        public string? Address { get; set; }
        public string? BuildingCompanyName { get; set; }
    }
}
