using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con apartamentos (Apartment).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar apartamentos en la base de datos.
    /// </summary>
    public class ApartmentService : IService<Apartment>
    {
        /// <summary>
        /// Repositorio de apartamentos utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Apartment> _apartmentRepository;
        /// <summary>
        /// Repositorio de plantas utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<Floor> _floorRepository;
        /// <summary>
        /// Repositorio de edificios utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<Building> _buildingRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de apartamentos.
        /// </summary>
        /// <param name="apartmentRepository">Repositorio de apartamentos.</param>
        /// <param name="floorRepository">Repositorio de plantas.</param>
        /// <param name="buildingRepository">Repositorio de edificios.</param>
        public ApartmentService(IRepository<Apartment> apartmentRepository, IRepository<Floor> floorRepository, IRepository<Building> buildingRepository)
        {
            _apartmentRepository = apartmentRepository;
            _floorRepository = floorRepository;
            _buildingRepository = buildingRepository;
        }

        /// <summary>
        /// Agrega un nuevo apartamento tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Apartment a agregar.</param>
        public void Add(Apartment entity)
        {
            if (_apartmentRepository.GetAll().Any(a => a.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un apartamento con ese código.");
            _apartmentRepository.Add(entity);
        }

        /// <summary>
        /// Elimina un apartamento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del apartamento a eliminar.</param>
        public void Delete(string id)
        {
            _ = _apartmentRepository.GetById(id) ?? throw new ArgumentException("El apartamento no existe.", nameof(id));
            _apartmentRepository.Delete(id);
        }

        /// <summary>
        /// Obtiene todos los apartamentos.
        /// </summary>
        /// <returns>Una colección de apartamentos.</returns>
        public IEnumerable<Apartment> GetAll()
        {
            return _apartmentRepository.GetAll();
        }

        /// <summary>
        /// Obtiene un apartamento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del apartamento.</param>
        /// <returns>El apartamento encontrado o null si no existe.</returns>
        public Apartment? GetById(string id)
        {
            return _apartmentRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene un apartamento por su código.
        /// </summary>
        /// <param name="code">Código del apartamento.</param>
        /// <returns>El apartamento encontrado o null si no existe.</returns>
        public Apartment? GetByCode(string code)
        {
            return _apartmentRepository.Find(a => a.Code == code);
        }

        /// <summary>
        /// Actualiza un apartamento existente.
        /// </summary>
        /// <param name="entity">Entidad Apartment a actualizar.</param>
        public void Update(Apartment entity)
        {

            _apartmentRepository.Update(entity);
        }
    }
}
