using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Enums;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un edificio, incluyendo sus datos principales y relaciones con otras entidades.
    /// Los comentarios originales del desarrollador han sido integrados y mejorados para estandarizar la documentaci�n.
    /// </summary>
    public class Building
    {
        /// <summary>
        /// Identificador �nico del edificio.
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
        /// Descripci�n del edificio.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// C�digo �nico del edificio.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Portal o n�mero de entrada del edificio.
        /// </summary>
        public string Doorway { get; set; }
        /// <summary>
        /// N�mero de plantas del edificio.
        /// </summary>
        public int FloorCount { get; set; }
        /// <summary>
        /// A�o de construcci�n del edificio.
        /// </summary>
        public int YearBuilt { get; set; }
        /// <summary>
        /// Precio del edificio.
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Certificado energético del edificio.
        /// </summary>
        public string EnergyCertificate { get; set; }
        /// <summary>
        /// Indica si el edificio tiene ascensor.
        /// </summary>
        public bool HasElevator { get; set; }
        /// <summary>
        /// Indica si el edificio tiene garaje.
        /// </summary>
        public bool HasGarage { get; set; }
        /// <summary>
        /// Identificador del estado del edificio.
        /// </summary>
        public string StatusId { get; set; }

        /// <summary>
        /// Número de apartamentos del edificio.
        /// </summary>
        public int ApartmentCount { get; set; }

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
        /// Colecci�n de direcciones asociadas al edificio.
        /// </summary>
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
        /// <summary>
        /// Colecci�n de plantas del edificio.
        /// </summary>
        public virtual ICollection<Floor> Floor { get; set; } = new List<Floor>();
        /// <summary>
        /// Colecci�n de im�genes del edificio.
        /// </summary>
        public virtual ICollection<BuildingImage> BuildingImage { get; set; } = new List<BuildingImage>();
        /// <summary>
        /// Colecci�n de logs de estado del edificio.
        /// </summary>
        public virtual ICollection<BuildingStatusLog> BuildingStatusLog { get; set; } = new List<BuildingStatusLog>();
        /// <summary>
        /// Colecci�n de compras asociadas al edificio.
        /// </summary>
        public virtual ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
        /// <summary>
        /// Colecci�n de solicitudes asociadas al edificio.
        /// </summary>
        public virtual ICollection<Request> Request { get; set; } = new List<Request>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Building() { }

        /// <summary>
        /// Construye el c�digo �nico del edificio usando el primer c�digo postal del distrito, el c�digo de la calle y el portal.
        /// </summary>
        /// <returns>El c�digo �nico generado para el edificio.</returns>
        public string BuildBuildingCode()
        {
            var zipCode = District?.Zipcode?.FirstOrDefault()?.Code ?? "ZZZ";
            var streetCode = Street?.Code ?? "000";
            var doorway = Doorway ?? "0";
            return $"{zipCode}-{streetCode}-{doorway}";
        }
    }
}