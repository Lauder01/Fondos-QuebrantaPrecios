namespace ClassLibraryProject.Entities
{
    public class Floor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Building? BuildingFloor { get; set; } = null;
        public int FloorNumber { get; set; } = -666;
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();
        public bool HasLift { get; set; } = false;
        
        public Floor() { }
    }
}

