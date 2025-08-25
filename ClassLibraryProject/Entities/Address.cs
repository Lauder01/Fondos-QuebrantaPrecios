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
        /// Identificador del edificio asociado a la dirección.
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// Identificador del apartamento asociado a la dirección (si aplica).
        /// </summary>
        public string? ApartmentId { get; set; }
        /// <summary>
        /// Identificador del código postal asociado a la dirección.
        /// </summary>
        public string ZipcodeId { get; set; }
        /// <summary>
        /// Dirección completa.
        /// </summary>
        public string ConstructedAddress { get; set; }

        /// <summary>
        /// Indica si la dirección corresponde a un apartamento.
        /// </summary>
        public bool? IsApartment { get; set; }
        /// <summary>
        /// País de la dirección.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Ciudad de la dirección.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Apartamento(entidad) al que pertenece la dirección.
        /// </summary>
        public virtual Apartment Apartment { get; set; }
        /// <summary>
        /// Edificio(entidad) al que pertenece la dirección.
        /// </summary>
        public virtual Building Building { get; set; }
        /// <summary>
        /// Zipcode(entidad) asociado a la dirección.
        /// </summary>
        public virtual Zipcode Zipcode { get; set; }
        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Address() { }
    }
}
