using System;
using System.Collections.Generic;

namespace WebAPI.Dtos.District
{
    public class DistrictGetterDto : DistrictBaseDto
    {
        public Guid Id { get; set; }
        // Hereda Zipcodes del DistrictBaseDto
    }
}
