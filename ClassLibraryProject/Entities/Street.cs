using FQP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FQP.Entities
{
    public class Street
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public StreetTypeEnum AddressStreetType { get; set; } = StreetTypeEnum.Undefined;

        public Street() { }

        public Street(string name, StreetTypeEnum streetType, string uniqueCode)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Name = name;
            AddressStreetType = streetType;
            Code = uniqueCode;
        }

        public Street(string name, StreetTypeEnum streetType)
        {
            Name = name;
            AddressStreetType = streetType;
            Code = BuildStreetCode();
        }

        public override string ToString() => GetComposedName();

        public string GetStreetTypeAcronym() => AddressStreetType switch
        {
            StreetTypeEnum.Calle => "C",
            StreetTypeEnum.Avenida => "Avda",
            StreetTypeEnum.Boulevard or StreetTypeEnum.Bulevar => "Blvr",
            StreetTypeEnum.Carretera => "Ctra",
            StreetTypeEnum.Paseo => "P",
            StreetTypeEnum.Plaza => "Pza",
            _ => string.Empty,
        };

        public string GetComposedName()
        {
            var acronym = GetStreetTypeAcronym();
            return string.IsNullOrEmpty(acronym)
                ? Name
                : $"{acronym}{(acronym == "P" ? ".º" : ".")} {Name}";
        }

        public string BuildStreetCode()
        {
            var acronym = GetStreetTypeAcronym();
            var initials = GetInitials(Name);
            return string.IsNullOrEmpty(acronym)
                ? initials
                : $"{acronym.ToUpper()}-{initials}";
        }

        public static string GetInitials(string input, int? lastWordLength = null)
        {
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join("-", words.Select((word, idx) =>
                idx == words.Length - 1 && lastWordLength.HasValue && lastWordLength.Value <= word.Length
                    ? word[..lastWordLength.Value].ToUpper()
                    : (word.Length >= 2 ? word[..2].ToUpper() : word.ToUpper())
            ));
        }
    }
}
