using System;
using FQP.Entities;

namespace FQP.Entities
{
    public class Request
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Building Building { get; set; } = new Building();
        public double Price { get; set; } = 0.0;

        public Request() { }
    }
}
