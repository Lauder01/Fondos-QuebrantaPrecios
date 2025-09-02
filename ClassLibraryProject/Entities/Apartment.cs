using System.Collections.Generic;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un apartamento dentro de una planta de un edificio.
    /// </summary>
    public class Apartment
    {
        /// <summary>
        /// Identificador �nico del apartamento.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// C�digo �nico del apartamento.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Puerta o n�mero del apartamento.
        /// </summary>
        public string Door { get; set; }
        /// <summary>
        /// Identificador de la planta donde se ubica el apartamento.
        /// </summary>
        public string FloorId { get; set; }
        /// <summary>
        /// Superficie del apartamento en m^2.
        /// </summary>
        public decimal Area { get; set; }
        /// <summary>
        /// Numero de habitaciones del apartamento.
        /// </summary>
        public int NumRooms { get; set; }
        /// <summary>
        /// Número de baños del apartamento.
        /// </summary>
        public int NumBathrooms { get; set; }
        /// <summary>
        /// Planta asociada al apartamento (navegaci�n).
        /// </summary>
        public virtual Floor Floor { get; set; }
        /// <summary>
        /// Colecci�n de direcciones asociada al apartamento (apartamento y edificio).
        /// </summary>
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Apartment() { }
    }
}
