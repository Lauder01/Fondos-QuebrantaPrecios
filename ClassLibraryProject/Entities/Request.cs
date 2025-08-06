using System;

namespace ClassLibraryProject.Entities
{
    public class Request
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BuildingId { get; set; }
        public Building RequestBuilding { get; set; } = default!;
        public Guid StatusId { get; set; }
        public Status RequestStatus { get; set; } = default!;
        public double Price { get; set; } = 0.0;

        public Request() { }
    }
}
