namespace ClassLibraryProject.Entities
{
	public class District
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? BuildingCount { get; set; }

        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        public virtual ICollection<Street> Street { get; set; } = new List<Street>();
        public virtual ICollection<Zipcode> Zipcode { get; set; } = new List<Zipcode>();

#pragma warning disable CS8618
        public District() { }
#pragma warning restore CS8618
    }
}
