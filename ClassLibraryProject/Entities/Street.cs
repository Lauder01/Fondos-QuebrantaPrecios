using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;

namespace ClassLibraryProject.Entities
{
    public class Street
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        public virtual ICollection<District> District { get; set; } = new List<District>();

        public Street() { }

        public Street(string baseName, StreetTypeEnum streetType, string code)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetComposedName(baseName, streetType);
            Code = code;
        }

        // El constructor antiguo se puede marcar como obsoleto o eliminar si no se usa
        [Obsolete("Usa el constructor con código único generado")]
        public Street(string baseName, StreetTypeEnum streetType)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetComposedName(baseName, streetType);
            Code = BuildStreetCode(baseName, streetType);
        }

        // Métodos utilitarios relacionados con la entidad
        public static string GetComposedName(string name, StreetTypeEnum streetType)
        {
            var acronym = streetType.GetAcronym();
            return string.IsNullOrEmpty(acronym)
                ? name
                : $"{acronym}{(acronym == "P" ? ".º" : ".")} {name}";
        }

        public static string BuildStreetCode(string name, StreetTypeEnum streetType)
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