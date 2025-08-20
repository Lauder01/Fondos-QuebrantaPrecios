using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con calles (Street).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar calles en la base de datos, así como para generar códigos únicos de calle.
    /// </summary>
    public class StreetService : IService<Street>
    {
        /// <summary>
        /// Repositorio de calles utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Street> _streetRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de calles.
        /// </summary>
        /// <param name="streetRepository">Repositorio de calles.</param>
        public StreetService(IRepository<Street> streetRepository)
        {
            _streetRepository = streetRepository;
        }

        /// <summary>
        /// Agrega una nueva calle tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Street a agregar.</param>
        public void Add(Street entity)
        {
            // Validación: Nombre compuesto requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre de la calle es obligatorio y debe tener entre 2 y 255 caracteres.");
            // Validación: Unicidad de nombre compuesto
            if (_streetRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una calle con ese nombre.");
            // Validación: Código único
            if (_streetRepository.GetAll().Any(s => s.Code == entity.Code))
                throw new InvalidOperationException("Ya existe una calle con ese código.");
            _streetRepository.Add(entity);
        }

        /// <summary>
        /// Elimina una calle por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la calle a eliminar.</param>
        public void Delete(string id)
        {
            _ = _streetRepository.GetById(id) ?? throw new ArgumentException("La calle no existe.", nameof(id));
            _streetRepository.Delete(id);
        }

        /// <summary>
        /// Obtiene todas las calles.
        /// </summary>
        /// <returns>Una colección de calles.</returns>
        public IEnumerable<Street> GetAll()
        {
            return _streetRepository.GetAll();
        }

        /// <summary>
        /// Obtiene una calle por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la calle.</param>
        /// <returns>La calle encontrada o null si no existe.</returns>
        public Street? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _streetRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene una calle por su código.
        /// </summary>
        /// <param name="code">Código de la calle.</param>
        /// <returns>La calle encontrada o null si no existe.</returns>
        public Street? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _streetRepository.Find(s => s.Code == code);
        }

        public Street? GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return _streetRepository.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Actualiza una calle existente.
        /// </summary>
        /// <param name="entity">Entidad Street a actualizar.</param>
        public void Update(Street entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La calle no puede ser nula.");
            _streetRepository.Update(entity);
        }

        /// <summary>
        /// Genera un código único para una calle basado en su nombre y tipo.
        /// </summary>
        /// <param name="baseName">Nombre base de la calle.</param>
        /// <param name="type">Tipo de calle.</param>
        /// <returns>Código único generado o null si no se puede generar.</returns>
        public string? GetUniqueStreetCode(string baseName, StreetTypeEnum type)
        {
            var existingCodes = new HashSet<string>(_streetRepository.GetAll().Select(s => s.Code));
            var words = baseName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int lastWordLength = 2;
            string code;
            do
            {
                var initials = Street.GetInitials(baseName, lastWordLength);
                var acronym = type.GetAcronym();
                code = string.IsNullOrEmpty(acronym) ? initials : $"{acronym}-{initials}";
                if (!existingCodes.Contains(code))
                    return code;
                if (words[^1].Length > lastWordLength)
                    lastWordLength++;
                else
                    return null;
            } while (true);
        }
    }
}
