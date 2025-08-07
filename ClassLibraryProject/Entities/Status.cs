using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    public class Status
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        public virtual ICollection<Request> Request { get; set; } = new List<Request>();
        public virtual ICollection<BuildingStatusLog> BuildingStatusLog { get; set; } = new List<BuildingStatusLog>();
        public virtual ICollection<RequestStatusLog> RequestStatusLog { get; set; } = new List<RequestStatusLog>();
        public Status() { }
    }
}
