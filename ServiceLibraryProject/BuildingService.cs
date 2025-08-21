using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con edificios (Building).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar edificios en la base de datos.
    /// </summary>
    public class BuildingService : IService<Building>
    {
        /// <summary>
        /// Repositorio de edificios utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Building> _buildingRepository;
        /// <summary>
        /// Repositorio de distritos utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<District> _districtRepository;
        /// <summary>
        /// Repositorio de calles utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<Street> _streetRepository;
        /// <summary>
        /// Repositorio de empresas constructoras utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<BuildingCompany> _companyRepository;
        /// <summary>
        /// Repositorio de estados utilizado para validaciones y relaciones.
        /// </summary>
        private readonly IRepository<Status> _statusRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de edificios.
        /// </summary>
        /// <param name="buildingRepository">Repositorio de edificios.</param>
        /// <param name="districtRepository">Repositorio de distritos.</param>
        /// <param name="streetRepository">Repositorio de calles.</param>
        /// <param name="companyRepository">Repositorio de empresas constructoras.</param>
        /// <param name="statusRepository">Repositorio de estados.</param>
        public BuildingService(
            IRepository<Building> buildingRepository,
            IRepository<District> districtRepository,
            IRepository<Street> streetRepository,
            IRepository<BuildingCompany> companyRepository,
            IRepository<Status> statusRepository)
        {
            _buildingRepository = buildingRepository;
            _districtRepository = districtRepository;
            _streetRepository = streetRepository;
            _companyRepository = companyRepository;
            _statusRepository = statusRepository;
        }

        /// <summary>
        /// Agrega un nuevo edificio tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Building a agregar.</param>
        public void Add(Building entity)
        {
            // Validación: Code único
            if (_buildingRepository.GetAll().Any(b => b.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un edificio con ese código.");
            // Validación: Price >= 0
            if (entity.Price < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            // Validación: District, Street, Company y Status existen
            if (entity.District == null || !_districtRepository.GetAll().Any(d => d.Id == entity.District.Id))
                throw new ArgumentException("El distrito asociado no existe.");
            if (entity.Street == null || !_streetRepository.GetAll().Any(s => s.Id == entity.Street.Id))
                throw new ArgumentException("La calle asociada no existe.");
            if (entity.BuildingCompany == null || !_companyRepository.GetAll().Any(c => c.Id == entity.BuildingCompany.Id))
                throw new ArgumentException("La empresa constructora asociada no existe.");
            if (entity.Status == null || !_statusRepository.GetAll().Any(s => s.Id == entity.Status.Id))
                throw new ArgumentException("El estado asociado no existe.");
            _buildingRepository.Add(entity);
        }

        /// <summary>
        /// Elimina un edificio por su identificador.
        /// </summary>
        /// <param name="id">Identificador del edificio a eliminar.</param>
        public void Delete(string id)
        {
            _ = _buildingRepository.GetById(id) ?? throw new ArgumentException("El edificio no existe.", nameof(id));
            _buildingRepository.Delete(id);
        }

        /// <summary>
        /// Obtiene todos los edificios.
        /// </summary>
        /// <returns>Una colección de edificios.</returns>
        public IEnumerable<Building> GetAll()
        {
            return _buildingRepository.GetAll();
        }

        /// <summary>
        /// Obtiene un edificio por su identificador.
        /// </summary>
        /// <param name="id">Identificador del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public Building? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _buildingRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene un edificio por su código.
        /// </summary>
        /// <param name="code">Código del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public Building? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _buildingRepository.Find(b => b.Code == code);
        }

        /// <summary>
        /// Actualiza un edificio existente.
        /// </summary>
        /// <param name="entity">Entidad Building a actualizar.</param>
        public void Update(Building entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El edificio no puede ser nulo.");
            _buildingRepository.Update(entity);
        }
    }
}
