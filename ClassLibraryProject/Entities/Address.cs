using System;

namespace ClassLibraryProject.Entities
{
    public class Address
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string ApartmentId { get; set; }
        public bool? IsApartment { get; set; }

        public Address() { }
    }
}
