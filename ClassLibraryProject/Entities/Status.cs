namespace ClassLibraryProject.Entities
{
    public class Status
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string StatusName { get; set; } = "Empty";
        public string? Description { get; set; } = "Empty";
    }
}
