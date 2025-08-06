using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;

namespace ClassLibraryProject.Entities
{
    public class Street
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Street() { }

        public Street(string baseName, StreetTypeEnum streetType)
        {
            Id = Guid.NewGuid();
            Name = GetComposedName(baseName, streetType);
            Code = BuildStreetCode(baseName, streetType);
        }

        private static string GetComposedName(string name, StreetTypeEnum streetType)
        {
            var acronym = streetType.GetAcronym();
            return string.IsNullOrEmpty(acronym)
                ? name
                : $"{acronym}{(acronym == "P" ? ".º" : ".")} {name}";
        }

        private static string BuildStreetCode(string name, StreetTypeEnum streetType)
        {
            var acronym = streetType.GetAcronym();
            var initials = GetInitials(name);
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
                    : word.Length >= 2 ? word[..2].ToUpper() : word.ToUpper()
            ));
        }
    }
}