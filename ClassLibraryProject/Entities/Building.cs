using System;
using ClassLibraryProject.Enums;

namespace ClassLibraryProject.Entities
{
    public class Building
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Street BuildingStreet { get; set; } = default!;
        public District BuildingDistrict { get; set; } = default!;
        public BuildingCompany BuildingCompany { get; set; } = default!;public string Code { get; set; } = string.Empty;
        public Status BuildingStatus { get; } = default!;
        public string Doorway { get; set; } = "0";
        public string? Name { get; set; } = "Empty";
        public string? Description { get; set; } = "Empty";
        public int? FloorCount { get; set; } = 0;
        public int? YearBuilt { get; set; } = 1970;
        public double? Price { get; set; } = 0.0;
        public EnergyCertificateEnum? BuildingEnergyCertificate { get; set; } = EnergyCertificateEnum.U;

        // Local
        public Address? BuildingAdress { get; } = null;
        public bool HasLift { get; } = true;
        public List<Floor> FloorList { get; } = new List<Floor>();

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