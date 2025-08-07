using System;

namespace ClassLibraryProject.Entities
{
    public class RequestStatusLog
    {
        public required string RequestId { get; set; }
        public required string StatusId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required virtual Request Request { get; set; }
        public required virtual Status Status { get; set; }
    
        public RequestStatusLog() { }
    }
}
