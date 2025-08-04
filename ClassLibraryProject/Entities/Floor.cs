namespace FQP.Entities
{
    public class Floor
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Building? BuildingFloor { get; set; } = null;
        public int FloorNumber { get; set; } = -666;
        public bool HasLift { get; set; } = false;

        // Local
        public List<Apartment> Apartments { get; } = new List<Apartment>();

        public Floor() { }
    }
}

