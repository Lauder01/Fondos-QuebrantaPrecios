using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryProject.Entities;
public partial class Zipcode
{
    public string Id { get; set; }

    public string Code { get; set; }

    public virtual ICollection<District> District { get; set; } = new List<District>();
}

