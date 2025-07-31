using FQP.Enums;

namespace FQP.Entities
{
    public class Street
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = string.Empty;
        public StreetTypeEnum AddressStreetType { get; set; } = StreetTypeEnum.Undefined;
        
        public Street() { }

        public override string ToString()
        {
            return AddressStreetType switch
            {
                StreetTypeEnum.Undefined => $"{Name}",
                StreetTypeEnum.Calle => $"C. {Name}",
                StreetTypeEnum.Avenida => $"Avda. {Name}",
                StreetTypeEnum.Boulevard => $"Blvr. {Name}",
                StreetTypeEnum.Bulevar => $"Blvr. {Name}",
                StreetTypeEnum.Carretera => $"Ctra. {Name}",
                StreetTypeEnum.Paseo => $"P.º {Name}",
                StreetTypeEnum.Plaza => $"Pza. {Name}",
                _ => $"{Name}",
            };
        }
    }
}
