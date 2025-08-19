using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.District
{
    public class DistrictGetterDto : DistrictBaseDto
    {
        public Guid Id { get; set; }
        public List<string> Zipcodes { get; set; } = new List<string>();
        // Hereda los demás campos del base
    }
}
