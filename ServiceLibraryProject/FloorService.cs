using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con plantas de edificio (Floor).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar plantas en la base de datos.
    /// </summary>
    public class FloorService : IService<Floor>
    {
        /// <summary>
        /// Repositorio de plantas utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Floor> _floorRepository;
        /// <summary>
        /// Repositorio de edificios utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<Building> _buildingRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de plantas.
        /// </summary>
        /// <param name="floorRepository">Repositorio de plantas.</param>
        /// <param name="buildingRepository">Repositorio de edificios.</param>
        public FloorService(IRepository<Floor> floorRepository, IRepository<Building> buildingRepository)
        {
            _floorRepository = floorRepository;
            _buildingRepository = buildingRepository;
        }

        /// <summary>
        /// Agrega una nueva planta tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Floor a agregar.</param>
        public void Add(Floor entity)
        {
            // Validación: BuildingId requerido y existencia
            if (entity.Building == null || !_buildingRepository.GetAll().Any(b => b.Id == entity.Building.Id))
                throw new ArgumentException("El edificio asociado no existe.");
            _floorRepository.Add(entity);
        }

        /// <summary>
        /// Elimina una planta por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la planta a eliminar.</param>
        public void Delete(string id)
        {
            _ = _floorRepository.GetById(id) ?? throw new ArgumentException("El piso no existe.", nameof(id));
            _floorRepository.Delete(id);
        }

        /// <summary>
        /// Obtiene todas las plantas.
        /// </summary>
        /// <returns>Una colección de plantas.</returns>
        public IEnumerable<Floor> GetAll()
        {
            return _floorRepository.GetAll();
        }

        /// <summary>
        /// Obtiene una planta por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la planta.</param>
        /// <returns>La planta encontrada o null si no existe.</returns>
        public Floor? GetById(string id)
        {
            return _floorRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene todas las plantas de un edificio específico.
        /// </summary>
        /// <param name="buildingId">Identificador del edificio.</param>
        /// <returns>Una colección de plantas del edificio especificado.</returns>
        public IEnumerable<Floor> GetByBuildingId(string buildingId)
        {
            return _floorRepository.GetAll().Where(f => f.BuildingId == buildingId);
        }

        /// <summary>
        /// Actualiza una planta existente.
        /// </summary>
        /// <param name="entity">Entidad Floor a actualizar.</param>
        public void Update(Floor entity)
        {
            _floorRepository.Update(entity);
        }
    }
}
