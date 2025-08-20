using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa el estado de un edificio o solicitud, incluyendo su descripción y relaciones.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Identificador único del estado.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del estado.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descripción del estado.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Colección de edificios asociados a este estado.
        /// </summary>
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        /// <summary>
        /// Colección de solicitudes asociadas a este estado.
        /// </summary>
        public virtual ICollection<Request> Request { get; set; } = new List<Request>();
        /// <summary>
        /// Colección de logs de estado de edificios.
        /// </summary>
        public virtual ICollection<BuildingStatusLog> BuildingStatusLog { get; set; } = new List<BuildingStatusLog>();
        /// <summary>
        /// Colección de logs de estado de solicitudes.
        /// </summary>
        public virtual ICollection<RequestStatusLog> RequestStatusLog { get; set; } = new List<RequestStatusLog>();
        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Status() { }
    }
}
