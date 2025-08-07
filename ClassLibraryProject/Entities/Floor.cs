namespace ClassLibraryProject.Entities
{
    public class Floor
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string Code { get; set; }
        public int FloorNumber { get; set; }
        public bool HasLift { get; set; }
        public virtual Building Building { get; set; }
        public virtual ICollection<Apartment> Apartment { get; set; } = new List<Apartment>();

        public Floor() { }

        public string BuildFloorCode()
        {
            var buildingCode = Building?.Code ?? "000";
            var floorNumber = FloorNumber.ToString("D2");
            return $"{buildingCode}-{floorNumber}";
        }
    }
}

