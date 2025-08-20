using System;
using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa una compra realizada sobre un edificio, incluyendo la empresa constructora y la solicitud asociada.
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// Identificador único de la compra.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Identificador del edificio asociado a la compra.
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// Identificador de la empresa constructora asociada a la compra.
        /// </summary>
        public string BuildingCompanyId { get; set; }
        /// <summary>
        /// Identificador de la solicitud asociada a la compra.
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// Fecha de la compra.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Importe de la compra.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Edificio asociado a la compra (navegación).
        /// </summary>
        public virtual Building Building { get; set; }
        /// <summary>
        /// Empresa constructora asociada a la compra (navegación).
        /// </summary>
        public virtual BuildingCompany BuildingCompany { get; set; }
        /// <summary>
        /// Solicitud asociada a la compra (navegación).
        /// </summary>
        public virtual Request Request { get; set; }
        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Purchase() { }
    }
}
