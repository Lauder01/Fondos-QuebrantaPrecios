using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            // Validación: Unicidad de nombre compuesto
            if (_streetRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una calle con ese nombre.");
            // Validación: Código único
            if (_streetRepository.GetAll().Any(s => s.Code == entity.Code))
                throw new InvalidOperationException("Ya existe una calle con ese código.");
            _streetRepository.Add(entity);
        }

        /// <summary>
        /// Agrega una nueva calle de manera asíncrona tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Street a agregar.</param>
        public async Task AddAsync(Street entity)
        {
            var all = await _streetRepository.GetAllAsync();
            // Validación: Unicidad de nombre compuesto
            if (all.Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una calle con ese nombre.");
            // Validación: Código único
            if (all.Any(s => s.Code == entity.Code))
                throw new InvalidOperationException("Ya existe una calle con ese código.");
            await _streetRepository.AddAsync(entity);
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
        /// Elimina una calle por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador de la calle a eliminar.</param>
        public async Task DeleteAsync(string id)
        {
            var street = await _streetRepository.GetByIdAsync(id);
            if (street == null)
                throw new ArgumentException("La calle no existe.", nameof(id));
            await _streetRepository.DeleteAsync(id);
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
        /// Obtiene todas las calles de manera asíncrona.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. Contiene una colección de calles.</returns>
        public async Task<IEnumerable<Street>> GetAllAsync()
        {
            return await _streetRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtiene una calle por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la calle.</param>
        /// <returns>La calle encontrada o null si no existe.</returns>
        public Street? GetById(string id)
        {
            return _streetRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene una calle por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador de la calle.</param>
        /// <returns>Una tarea que representa la operación asíncrona. Contiene la calle encontrada o null si no existe.</returns>
        public async Task<Street?> GetByIdAsync(string id)
        {
            return await _streetRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Obtiene una calle por su código.
        /// </summary>
        /// <param name="code">Código de la calle.</param>
        /// <returns>La calle encontrada o null si no existe.</returns>
        public Street? GetByCode(string code)
        {
            return _streetRepository.Find(s => s.Code == code);
        }

        /// <summary>
        /// Obtiene una calle por su código de manera asíncrona.
        /// </summary>
        /// <param name="code">Código de la calle.</param>
        /// <returns>Una tarea que representa la operación asíncrona. Contiene la calle encontrada o null si no existe.</returns>
        public async Task<Street?> GetByCodeAsync(string code)
        {
            return await _streetRepository.FindAsync(s => s.Code == code);
        }

        /// <summary>
        /// Obtiene una calle por su nombre.
        /// Realiza una búsqueda en el repositorio de calles utilizando una comparación que no distingue mayúsculas ni minúsculas.
        /// Si el nombre proporcionado es nulo o está vacío, retorna null.
        /// </summary>
        public Street? GetByName(string name)
        {
            return _streetRepository.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtiene una calle por su nombre de manera asíncrona.
        /// Realiza una búsqueda en el repositorio de calles utilizando una comparación que no distingue mayúsculas ni minúsculas.
        /// Si el nombre proporcionado es nulo o está vacío, retorna null.
        /// </summary>
        public async Task<Street?> GetByNameAsync(string name)
        {
            return await _streetRepository.FindAsync(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Actualiza una calle existente.
        /// </summary>
        /// <param name="entity">Entidad Street a actualizar.</param>
        public void Update(Street entity)
        {
            _streetRepository.Update(entity);
        }

        /// <summary>
        /// Actualiza una calle existente de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad Street a actualizar.</param>
        public async Task UpdateAsync(Street entity)
        {
            await _streetRepository.UpdateAsync(entity);
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
