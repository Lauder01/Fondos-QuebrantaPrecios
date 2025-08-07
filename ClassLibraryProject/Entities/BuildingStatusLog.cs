using System;

namespace ClassLibraryProject.Entities
{
    public class BuildingStatusLog
    {
        public required string BuildingId { get; set; }
        public required string StatusId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required virtual Building Building { get; set; }
        public required virtual Status Status { get; set; }

        public BuildingStatusLog() { }
    }
}
