using ClassLibraryProject.Enums;
using System;

namespace ClassLibraryProject.Extensions
{
    public static class StreetTypeEnumExtensions
    {
        private static readonly string[] Acronyms =
        {
            "C",            // Calle = 0
            "Avda",         // Avenida = 1
            "Blvr",         // Boulevard = 2
            "Blvr",         // Bulevar = 3
            "Ctra",         // Carretera = 4
            "P",            // Paseo = 5
            "Pza",          // Plaza = 6
            string.Empty    // Undefined = 7
        };

        public static string GetAcronym(this StreetTypeEnum streetType)
        {
            int index = (int)streetType;
            return index >= 0 && index < Acronyms.Length ? Acronyms[index] : string.Empty;
        }
    }
}
