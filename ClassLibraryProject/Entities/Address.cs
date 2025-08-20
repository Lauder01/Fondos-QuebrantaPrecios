using System;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa una dirección asociada a un edificio o apartamento.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Identificador único de la dirección.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Identificador del código postal asociado a la dirección.
        /// </summary>
        public string ZipcodeId { get; set; }
        /// <summary>
        /// Identificador de la calle asociada a la dirección.
        /// </summary>
        public string StreetId { get; set; }
        /// <summary>
        /// Identificador del edificio asociado a la dirección.
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// Identificador del apartamento asociado a la dirección (si aplica).
        /// </summary>
        public string ApartmentId { get; set; }
        /// <summary>
        /// Indica si la dirección corresponde a un apartamento.
        /// </summary>
        public bool? IsApartment { get; set; }

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Address() { }
    }
}
