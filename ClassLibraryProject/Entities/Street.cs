using System;
using System.ComponentModel.DataAnnotations;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa una calle de la ciudad, incluyendo su código, nombre y relaciones con edificios y distritos.
    /// </summary>
    public class Street
    {
        /// <summary>
        /// Identificador único de la calle.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Código único de la calle.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Nombre de la calle.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Colección de edificios ubicados en esta calle.
        /// </summary>
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        /// <summary>
        /// Colección de distritos que atraviesan esta calle.
        /// </summary>
        public virtual ICollection<District> District { get; set; } = new List<District>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Street() { }

        /// <summary>
        /// Constructor que inicializa una calle con nombre, tipo y código.
        /// </summary>
        /// <param name="baseName">Nombre base de la calle.</param>
        /// <param name="streetType">Tipo de calle.</param>
        /// <param name="code">Código único de la calle.</param>
        public Street(string baseName, StreetTypeEnum streetType, string code)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetComposedName(baseName, streetType);
            Code = code;
        }

        /// <summary>
        /// Constructor obsoleto, usar el constructor con código único generado.
        /// </summary>
        /// <param name="baseName">Nombre base de la calle.</param>
        /// <param name="streetType">Tipo de calle.</param>
        [Obsolete("Usa el constructor con código único generado")]
        public Street(string baseName, StreetTypeEnum streetType)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetComposedName(baseName, streetType);
            Code = BuildStreetCode(baseName, streetType);
        }

        /// <summary>
        /// Devuelve el nombre compuesto de la calle según el tipo.
        /// </summary>
        /// <param name="name">Nombre base de la calle.</param>
        /// <param name="streetType">Tipo de calle.</param>
        /// <returns>Nombre compuesto de la calle.</returns>
        public static string GetComposedName(string name, StreetTypeEnum streetType)
        {
            var acronym = streetType.GetAcronym();
            return string.IsNullOrEmpty(acronym)
                ? name
                : $"{acronym}{(acronym == "P" ? ".º" : ".")} {name}";
        }

        /// <summary>
        /// Construye el código único de la calle a partir del nombre y tipo.
        /// </summary>
        /// <param name="name">Nombre base de la calle.</param>
        /// <param name="streetType">Tipo de calle.</param>
        /// <returns>Código único generado para la calle.</returns>
        public static string BuildStreetCode(string name, StreetTypeEnum streetType)
        {
            var acronym = streetType.GetAcronym();
            var initials = GetInitials(name);
            return string.IsNullOrEmpty(acronym)
                ? initials
                : $"{acronym.ToUpper()}-{initials}";
        }

        /// <summary>
        /// Obtiene las iniciales del nombre para el código de la calle.
        /// </summary>
        /// <param name="input">Nombre base de la calle.</param>
        /// <param name="lastWordLength">Longitud de la última palabra (opcional).</param>
        /// <returns>Iniciales generadas para el código de la calle.</returns>
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