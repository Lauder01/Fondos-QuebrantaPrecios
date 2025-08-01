using System;
using FQP.Enums;

namespace FQP.Entities
{
    public class Building
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Name { get; set; } = "Empty";
        public string Doorway { get; set; } = "0";
        public District? BuildingDistrict { get; set; } = null;
        public Address? BuildingAdress { get; set; } = null;
        public string? Code { get; set; }
        public Company? BuildingCompany { get; set; } = null;
        public double? Price { get; set; } = 0.0;
        public Status? Status { get; set; } = null;
        public EnergyCertificateEnum? BuildingEnergyCertificate { get; set; } = EnergyCertificateEnum.U;
        public List<Floor> FloorCount { get; set; } = new List<Floor>();
        public int? YearBuilt { get; set; } = 1;
        public string? Description { get; set; } = "Empty";
        public bool HasLift { get; set; } = false;
        public bool IsCompanyBuilding { get; set; } = false;

        public Building() { }

        public string BuildBuildingCode()
        {
            var zipCode = BuildingAdress?.AddressZipCode ?? "ZZZ";
            var streetCode = BuildingAdress?.AddressStreet?.Code ?? "XXX";
            var doorway = Doorway ?? "0";
            return $"{zipCode}-{streetCode}-{doorway}";
        }
    }
}