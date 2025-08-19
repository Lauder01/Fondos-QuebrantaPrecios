using System;
using ClassLibraryProject.Enums;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibraryProject.Entities
{
    public class Building
    {
        // Escalares
        public string Id { get; set; }
        public string DistrictId { get; set; }
        public string StreetId { get; set; }
        public string BuildingCompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Doorway { get; set; }
        public int FloorCount { get; set; }
        public int YearBuilt { get; set; }
        public decimal Price { get; set; }
        public string EnergyCertificate { get; set; }
        public string StatusId { get; set; }

        // Propiedades de navegación
        public virtual District District { get; set; }
        public virtual Street Street { get; set; }
        public virtual BuildingCompany BuildingCompany { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
        public virtual ICollection<Floor> Floor { get; set; } = new List<Floor>();
        public virtual ICollection<BuildingImage> BuildingImage { get; set; } = new List<BuildingImage>();
        public virtual ICollection<BuildingStatusLog> BuildingStatusLog { get; set; } = new List<BuildingStatusLog>();
        public virtual ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
        public virtual ICollection<Request> Request { get; set; } = new List<Request>();

        public Building() { }

        public string BuildBuildingCode()
        {
            // Usar el primer código postal asociado al distrito, si existe
            var zipCode = District?.Zipcode?.FirstOrDefault()?.Code ?? "ZZZ";
            var streetCode = Street?.Code ?? "000";
            var doorway = Doorway ?? "0";
            return $"{zipCode}-{streetCode}-{doorway}";
        }
    }
}