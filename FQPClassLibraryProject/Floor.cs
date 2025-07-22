using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQPClassLibraryProject
{
    public class Floor
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public int FloorNumber { get; set; } = -666;
        public List<Apartment> apartments { get; set; } = new List<Apartment>();
        public bool HasLift { get; set; } = false;
        
        public Floor() { }
    }
}

