namespace WebAPI.Dtos.Status
{
    public class StatusGetterDto : StatusBaseDto
    {
        // Hereda todos los campos del base, incluido Id
        public string Id { get; set; }
    }
}
