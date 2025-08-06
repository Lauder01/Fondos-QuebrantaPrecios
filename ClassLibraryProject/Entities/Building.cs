using System;
using ClassLibraryProject.Enums;
using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    public class Building
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StreetId { get; set; }
        public Street BuildingStreet { get; set; } = default!;
        public Guid DistrictId { get; set; }
        public District BuildingDistrict { get; set; } = default!;
        public Guid CompanyId { get; set; }
        public BuildingCompany BuildingCompany { get; set; } = default!;
        public string Code { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        public Status BuildingStatus { get; set; } = default!;
        public string Doorway { get; set; } = "0";
        public string? Name { get; set; } = "Empty";
        public string? Description { get; set; } = "Empty";
        public int? FloorCount { get; set; } = 0;
        public int? YearBuilt { get; set; } = 1970;
        public double? Price { get; set; } = 0.0;
        public EnergyCertificateEnum? BuildingEnergyCertificate { get; set; } = EnergyCertificateEnum.U;
        public Guid? AddressId { get; set; }
        public Address? BuildingAdress { get; set; } = null;
        public bool HasLift { get; set; } = true;
        public List<Floor> FloorList { get; set; } = new List<Floor>();
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();

        public Building() { }

        public string BuildBuildingCode()
        {
            var zipCode = BuildingDistrict?.ZipCode ?? "ZZZ";
            var streetCode = BuildingStreet?.Code ?? "000";
            var doorway = Doorway ?? "0";
            return $"{zipCode}-{streetCode}-{doorway}";
        }
    }
}