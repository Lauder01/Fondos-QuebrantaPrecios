namespace FQP.Entities
{
    public class Floor
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Building? BuildingFloor { get; set; } = null;
        public string Code { get; set; } = string.Empty;
        public int FloorNumber { get; set; } = -12;
        public bool HasLift { get; set; } = false;

        // Local
        public List<Apartment> Apartments { get; } = new List<Apartment>();

        public Floor() { }

        public string BuildFloorCode()
        {
            var buildingCode = BuildingFloor?.Code ?? "000";
            var floorNumber = FloorNumber.ToString("D2");
            return $"{buildingCode}-{floorNumber}";
        }
    }
}

