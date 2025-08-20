using System;
using System.Collections.Generic;
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
        /// Obtiene una solicitud por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la solicitud.</param>
        /// <returns>La solicitud encontrada o null si no existe.</returns>
        public Request? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _requestRepository.GetById(id);
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
        /// Actualiza una solicitud existente.
        /// </summary>
        /// <param name="entity">Entidad Request a actualizar.</param>
        public void Update(Request entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La solicitud no puede ser nula.");
            _requestRepository.Update(entity);
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
    }
}
