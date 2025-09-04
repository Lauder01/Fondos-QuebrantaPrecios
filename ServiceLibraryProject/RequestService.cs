using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con solicitudes (Request).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar solicitudes en la base de datos.
    /// </summary>
    public class RequestService : IService<Request>
    {
        /// <summary>
        /// Repositorio de solicitudes utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Request> _requestRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de solicitudes.
        /// </summary>
        /// <param name="requestRepository">Repositorio de solicitudes.</param>
        public RequestService(IRepository<Request> requestRepository)
        {
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Obtiene todas las solicitudes.
        /// </summary>
        /// <returns>Una colección de solicitudes.</returns>
        public IEnumerable<Request> GetAll()
        {
            return _requestRepository.GetAll();
        }

        /// <summary>
        /// Obtiene todas las solicitudes de manera asíncrona.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. Su resultado contiene una colección de solicitudes.</returns>
        public async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _requestRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtiene una solicitud por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la solicitud.</param>
        /// <returns>La solicitud encontrada o null si no existe.</returns>
        public Request? GetById(string id)
        {
            return _requestRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene una solicitud por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador de la solicitud.</param>
        /// <returns>Una tarea que representa la operación asíncrona. Su resultado contiene la solicitud encontrada o null si no existe.</returns>
        public async Task<Request?> GetByIdAsync(string id)
        {
            return await _requestRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Agrega una nueva solicitud.
        /// </summary>
        /// <param name="entity">Entidad Request a agregar.</param>
        public void Add(Request entity)
        {
            _requestRepository.Add(entity);
        }

        /// <summary>
        /// Agrega una nueva solicitud de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad Request a agregar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(Request entity)
        {
            await _requestRepository.AddAsync(entity);
        }

        /// <summary>
        /// Actualiza una solicitud existente.
        /// </summary>
        /// <param name="entity">Entidad Request a actualizar.</param>
        public void Update(Request entity)
        {
            _requestRepository.Update(entity);
        }

        /// <summary>
        /// Actualiza una solicitud existente de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad Request a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task UpdateAsync(Request entity)
        {
            await _requestRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Elimina una solicitud por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la solicitud a eliminar.</param>
        public void Delete(string id)
        {
            _ = _requestRepository.GetById(id) ?? throw new ArgumentException("La solicitud no existe.", nameof(id));
            _requestRepository.Delete(id);
        }

        /// <summary>
        /// Elimina una solicitud por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador de la solicitud a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task DeleteAsync(string id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
                throw new ArgumentException("La solicitud no existe.", nameof(id));
            await _requestRepository.DeleteAsync(id);
        }
    }
}
