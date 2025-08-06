using System;

namespace ClassLibraryProject.Entities
{
    public class Status
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Empty";
        public string? Description { get; set; } = "Empty";
    }
}
