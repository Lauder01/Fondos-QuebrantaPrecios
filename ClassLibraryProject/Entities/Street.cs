using FQP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FQP.Entities
{
    public class Street
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // Local
        public StreetTypeEnum AddressStreetType { get; set; } = StreetTypeEnum.Undefined;

        public Street() { }

        public Street(string name, StreetTypeEnum streetType)
        {
            Id = Guid.NewGuid();
            Name = GetComposedName();
            AddressStreetType = streetType;
            Code = BuildStreetCode();
        }

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