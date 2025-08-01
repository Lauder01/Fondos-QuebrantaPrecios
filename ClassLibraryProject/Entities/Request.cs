using System;
using FQP.Entities;

namespace FQP.Entities
{
    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Status? Status { get; set; }
        public Building Building { get; set; } = new Building();
        public double Price { get; set; } = 0.0;
        public double? MaintenancePrice { get; set; }

        public Request() { }

        public Request(Status? status, Building building)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Status = status;
            Building = building;
            Price = Building.Price ?? 0.0;
            MaintenancePrice = 0.0;
        }
    }
}
