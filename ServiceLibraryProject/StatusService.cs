using System;
using System.Collections.Generic;
using System.Linq;
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
            // Validación: Nombre requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length > 48)
                throw new ArgumentException("El nombre del estado es obligatorio y debe tener como máximo 48 caracteres.");
            // Validación: Unicidad de nombre
            if (_statusRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un estado con ese nombre.");
            _statusRepository.Add(entity);
        }

        /// <summary>
        /// Elimina un estado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del estado a eliminar.</param>
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene todos los estados.
        /// </summary>
        /// <returns>Una colección de estados.</returns>
        public IEnumerable<Status> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene un estado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del estado.</param>
        /// <returns>El estado encontrado o null si no existe.</returns>
        public Status? GetById(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualiza un estado existente.
        /// </summary>
        /// <param name="entity">Entidad Status a actualizar.</param>
        public void Update(Status entity)
        {
            throw new NotImplementedException();
        }
    }
}
