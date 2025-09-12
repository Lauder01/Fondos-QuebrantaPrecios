using System;
using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    public class Request
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public decimal Price { get; set; }
        public float MaintenancePrice { get; set; }
        public string StatusId { get; set; }
        public virtual Building Building { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<RequestStatusLog> RequestStatusLog { get; set; } = new List<RequestStatusLog>();
        public Request() { }
    }
}
