namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa una planta de un edificio, incluyendo su número, código y relación con apartamentos.
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// Identificador único de la planta.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Identificador del edificio al que pertenece la planta.
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// Código único de la planta.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Número de la planta (puede ser negativo para sótanos).
        /// </summary>
        public int FloorNumber { get; set; }
        /// <summary>
        /// Indica si la planta tiene ascensor.
        /// </summary>
        public bool HasLift { get; set; }
        /// <summary>
        /// Edificio asociado a la planta (navegación).
        /// </summary>
        public virtual Building Building { get; set; }
        /// <summary>
        /// Colección de apartamentos en la planta.
        /// </summary>
        public virtual ICollection<Apartment> Apartment { get; set; } = new List<Apartment>();

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public Floor() { }

        /// <summary>
        /// Construye el código único de la planta usando el código del edificio y el número de planta.
        /// </summary>
        /// <returns>El código único generado para la planta.</returns>
        public string BuildFloorCode()
        {
            var buildingCode = Building?.Code ?? "000";
            var floorNumber = FloorNumber.ToString("D2");
            return $"{buildingCode}-{floorNumber}";
        }
    }
}

