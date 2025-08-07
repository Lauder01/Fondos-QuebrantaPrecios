using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    public class Apartment
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Door { get; set; }
        public string FloorId { get; set; }
        public virtual Floor Floor { get; set; }
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
        public Apartment() { }
    }
}
