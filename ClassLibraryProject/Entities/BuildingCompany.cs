using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    public class BuildingCompany
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cif { get; set; }
        public string Website { get; set; }
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        public virtual ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
        public BuildingCompany() { }
    }
}
