using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa una empresa constructora, incluyendo sus datos y relaciones con edificios y compras.
    /// </summary>
    public class BuildingCompany
    {
        /// <summary>
        /// Identificador único de la empresa constructora.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la empresa constructora.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// CIF de la empresa constructora.
        /// </summary>
        public string Cif { get; set; }
        /// <summary>
        /// Sitio web de la empresa constructora.
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Colección de edificios construidos por la empresa.
        /// </summary>
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public BuildingCompany() { }
    }
}
