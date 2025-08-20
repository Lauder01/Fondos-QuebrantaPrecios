using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Enums;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un edificio, incluyendo sus datos principales y relaciones con otras entidades.
    /// Los comentarios originales del desarrollador han sido integrados y mejorados para estandarizar la documentación.
    /// </summary>
    public class Building
    {
        /// <summary>
        /// Identificador único del edificio.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Identificador del distrito al que pertenece el edificio.
        /// </summary>
        public string DistrictId { get; set; }
        /// <summary>
        /// Identificador de la calle donde se ubica el edificio.
        /// </summary>
        public string StreetId { get; set; }
        /// <summary>
        /// Identificador de la empresa constructora.
        /// </summary>
        public string BuildingCompanyId { get; set; }
        /// <summary>
        /// Nombre del edificio.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descripción del edificio.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Código único del edificio.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Portal o número de entrada del edificio.
        /// </summary>
        public string Doorway { get; set; }
        /// <summary>
        /// Número de plantas del edificio.
        /// </summary>
        public int FloorCount { get; set; }
        /// <summary>
        /// Año de construcción del edificio.
        /// </summary>
        public int YearBuilt { get; set; }
        /// <summary>
        /// Precio del edificio.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Certificado energético del edificio.
        /// </summary>
        public string EnergyCertificate { get; set; }
        /// <summary>
        /// Identificador del estado del edificio.
        /// </summary>
        public string StatusId { get; set; }

        /// <summary>
        /// Distrito asociado al edificio (navegación).
        /// </summary>
        public virtual District District { get; set; }
        /// <summary>
        /// Calle asociada al edificio (navegación).
        /// </summary>
        public virtual Street Street { get; set; }
        /// <summary>
        /// Empresa constructora asociada al edificio (navegación).
        /// </summary>
        public virtual BuildingCompany BuildingCompany { get; set; }
        /// <summary>
        /// Estado asociado al edificio (navegación).
        /// </summary>
        public virtual Status Status { get; set; }
        /// <summary>
        /// Colección de direcciones asociadas al edificio.
        /// </summary>
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
        /// <summary>
        /// Colección de plantas del edificio.
        /// </summary>
        public virtual ICollection<Floor> Floor { get; set; } = new List<Floor>();
        /// <summary>
        /// Colección de imágenes del edificio.
        /// </summary>
        public virtual ICollection<BuildingImage> BuildingImage { get; set; } = new List<BuildingImage>();
        /// <summary>
        /// Colección de logs de estado del edificio.
        /// </summary>
        public virtual ICollection<BuildingStatusLog> BuildingStatusLog { get; set; } = new List<BuildingStatusLog>();
        /// <summary>
        /// Colección de compras asociadas al edificio.
        /// </summary>
        public virtual ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
        /// <summary>
        /// Colección de solicitudes asociadas al edificio.
        /// </summary>
        public virtual ICollection<Request> Request { get; set; } = new List<Request>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Building() { }

        /// <summary>
        /// Construye el código único del edificio usando el primer código postal del distrito, el código de la calle y el portal.
        /// </summary>
        /// <returns>El código único generado para el edificio.</returns>
        public string BuildBuildingCode()
        {
            var zipCode = District?.Zipcode?.FirstOrDefault()?.Code ?? "ZZZ";
            var streetCode = Street?.Code ?? "000";
            var doorway = Doorway ?? "0";
            return $"{zipCode}-{streetCode}-{doorway}";
        }
    }
}