namespace ClassLibraryProject.Entities
{
	public class District
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? BuildingCount { get; set; }

        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        public virtual ICollection<Street> Street { get; set; } = new List<Street>();

        public District() { }
	}
}
