namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un distrito de la ciudad, incluyendo sus edificios, calles y códigos postales asociados.
    /// </summary>
    public class District
    {
        /// <summary>
        /// Identificador único del distrito.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del distrito.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Código único del distrito.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// País donde se encuentra el distrito.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Ciudad donde se encuentra el distrito.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Número de edificios asociados al distrito.
        /// </summary>
        public int? BuildingCount { get; set; }
        /// <summary>
        /// Colección de edificios que pertenecen a este distrito.
        /// </summary>
        public virtual ICollection<Building> Building { get; set; } = new List<Building>();
        /// <summary>
        /// Colección de calles que atraviesan este distrito.
        /// </summary>
        public virtual ICollection<Street> Street { get; set; } = new List<Street>();
        /// <summary>
        /// Colección de códigos postales asociados al distrito.
        /// </summary>
        public virtual ICollection<Zipcode> Zipcode { get; set; } = new List<Zipcode>();

#pragma warning disable CS8618
        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public District() { }
#pragma warning restore CS8618

        /// <summary>
        /// Devuelve la lista de códigos postales asociados al distrito.
        /// </summary>
        /// <returns>Lista de códigos postales.</returns>
        public List<string> GetZipCodes()
        {
            return Zipcode?.Select(z => z.Code).ToList() ?? new List<string>();
        }
    }
}
