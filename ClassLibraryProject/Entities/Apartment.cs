using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un apartamento dentro de una planta de un edificio.
    /// </summary>
    public class Apartment
    {
        /// <summary>
        /// Identificador único del apartamento.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Código único del apartamento.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Puerta o número del apartamento.
        /// </summary>
        public string Door { get; set; }
        /// <summary>
        /// Identificador de la planta donde se ubica el apartamento.
        /// </summary>
        public string FloorId { get; set; }
        /// <summary>
        /// Planta asociada al apartamento (navegación).
        /// </summary>
        public virtual Floor Floor { get; set; }
        /// <summary>
        /// Colección de direcciones asociadas al apartamento.
        /// </summary>
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Apartment() { }
    }
}
