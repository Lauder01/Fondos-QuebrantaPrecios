using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con estados (Status).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar estados en la base de datos.
    /// </summary>
    public class StatusService : IService<Status>
    {
        /// <summary>
        /// Repositorio de estados utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Status> _statusRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de estados.
        /// </summary>
        /// <param name="statusRepository">Repositorio de estados.</param>
        public StatusService(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        /// <summary>
        /// Agrega un nuevo estado tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Status a agregar.</param>
        public void Add(Status entity)
        {
            // Validación: Unicidad de nombre
            if (_statusRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un estado con ese nombre.");
            _statusRepository.Add(entity);
        }

        /// <summary>
        /// Agrega un nuevo estado de manera asíncrona tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Status a agregar.</param>
        public async Task AddAsync(Status entity)
        {
            var all = await _statusRepository.GetAllAsync();
            if (all.Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un estado con ese nombre.");
            await _statusRepository.AddAsync(entity);
        }

        /// <summary>
        /// Elimina un estado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del estado a eliminar.</param>
        public void Delete(string id)
        {
            _statusRepository.Delete(id);
        }

        /// <summary>
        /// Elimina un estado por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador del estado a eliminar.</param>
        public async Task DeleteAsync(string id)
        {
            await _statusRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Obtiene todos los estados.
        /// </summary>
        /// <returns>Una colección de estados.</returns>
        public IEnumerable<Status> GetAll()
        {
            return _statusRepository.GetAll();
        }

        /// <summary>
        /// Obtiene todos los estados de manera asíncrona.
        /// </summary>
        /// <returns>Una colección de estados.</returns>
        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _statusRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtiene un estado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del estado.</param>
        /// <returns>El estado encontrado o null si no existe.</returns>
        public Status? GetById(string id)
        {
            return _statusRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene un estado por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador del estado.</param>
        /// <returns>El estado encontrado o null si no existe.</returns>
        public async Task<Status?> GetByIdAsync(string id)
        {
            return await _statusRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Actualiza un estado existente.
        /// </summary>
        /// <param name="entity">Entidad Status a actualizar.</param>
        public void Update(Status entity)
        {
            _statusRepository.Update(entity);
        }

        /// <summary>
        /// Actualiza un estado existente de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad Status a actualizar.</param>
        public async Task UpdateAsync(Status entity)
        {
            await _statusRepository.UpdateAsync(entity);
        }
    }
}
