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
            /// Repositorio de direcciones utilizado para crear direcciones automáticamente.
            /// </summary>
            private readonly IRepository<Address> _addressRepository;

            /// <summary>
            /// Inicializa una nueva instancia del servicio de edificios.
            /// </summary>
            /// <param name="buildingRepository">Repositorio de edificios.</param>
            /// <param name="districtRepository">Repositorio de distritos.</param>
            /// <param name="streetRepository">Repositorio de calles.</param>
            /// <param name="companyRepository">Repositorio de empresas constructoras.</param>
            /// <param name="statusRepository">Repositorio de estados.</param>
            /// <param name="addressRepository">Repositorio de direcciones.</param>
            public BuildingService(
                IRepository<Building> buildingRepository,
                IRepository<District> districtRepository,
                IRepository<Street> streetRepository,
                IRepository<BuildingCompany> companyRepository,
                IRepository<Status> statusRepository,
                IRepository<Address> addressRepository)
            {
                _buildingRepository = buildingRepository;
                _districtRepository = districtRepository;
                _streetRepository = streetRepository;
                _companyRepository = companyRepository;
                _statusRepository = statusRepository;
                _addressRepository = addressRepository;
            }

            /// <summary>
            /// Obtiene una lista paginada y filtrada de edificios.
            /// </summary>
            /// <param name="page">Página actual</param>
            /// <param name="pageSize">Tamaño de página</param>
            /// <param name="name">Filtro por nombre</param>
            /// <param name="districtId">Filtro por distrito</param>
            /// <param name="companyId">Filtro por empresa constructora</param>
            /// <returns>Tupla con la lista de edificios y el total</returns>
        /// <summary>
        /// Obtiene una lista paginada y filtrada de edificios.
        /// </summary>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="name">Filtro por nombre</param>
        /// <param name="districtId">Filtro por distrito</param>
        /// <param name="companyId">Filtro por empresa constructora</param>
        /// <returns>Tupla con la lista de edificios y el total</returns>
        public (IEnumerable<Building> Items, int TotalCount) GetPagedAndFiltered(int page, int pageSize, string? name, string? districtId, string? companyId)
        {
            var query = GetAllWithAddressAndDistrict().AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(b => b.Name != null && b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(districtId))
                query = query.Where(b => b.DistrictId == districtId);
            if (!string.IsNullOrWhiteSpace(companyId))
                query = query.Where(b => b.BuildingCompanyId == companyId);
            var total = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (items, total);
        }

        /// <summary>
        /// Obtiene una lista paginada y filtrada de edificios de manera asíncrona.
        /// </summary>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="name">Filtro por nombre</param>
        /// <param name="districtId">Filtro por distrito</param>
        /// <param name="companyId">Filtro por empresa constructora</param>
        /// <returns>Tupla con la lista de edificios y el total</returns>
        public async Task<(IEnumerable<Building> Items, int TotalCount)> GetPagedAndFilteredAsync(int page, int pageSize, string? name, string? districtId, string? companyId)
        {
            var all = await GetAllWithAddressAndDistrictAsync();
            var query = all.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(b => b.Name != null && b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(districtId))
                query = query.Where(b => b.DistrictId == districtId);
            if (!string.IsNullOrWhiteSpace(companyId))
                query = query.Where(b => b.BuildingCompanyId == companyId);
            var total = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (items, total);
        }

        /// <summary>
        /// Agrega un nuevo edificio tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Building a agregar.</param>
        public void Add(Building entity)
        {
            // Sanitizar strings nulos/undefined a string vacío
            entity.Name = entity.Name ?? string.Empty;
            entity.Description = entity.Description ?? string.Empty;
            entity.Code = entity.Code ?? string.Empty;
            entity.Doorway = entity.Doorway ?? string.Empty;
            entity.DistrictId = entity.DistrictId ?? string.Empty;
            entity.StreetId = entity.StreetId ?? string.Empty;
            entity.BuildingCompanyId = entity.BuildingCompanyId ?? string.Empty;
            entity.StatusId = entity.StatusId ?? string.Empty;
            entity.EnergyCertificate = entity.EnergyCertificate ?? string.Empty;
            
            // Validación: Code único
            if (_buildingRepository.GetAll().Any(b => b.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un edificio con ese código.");
            // Validación: Nombre único (solo si se proporciona un nombre)
            if (!string.IsNullOrEmpty(entity.Name) && _buildingRepository.GetAll().Any(b => b.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un edificio con ese nombre.");
                
            // Validación: District, Street, Company y Status existen (solo si se proporcionan)
            if (!string.IsNullOrEmpty(entity.DistrictId) && !_districtRepository.GetAll().Any(d => d.Id == entity.DistrictId))
                throw new ArgumentException("El distrito asociado no existe.");
            if (!string.IsNullOrEmpty(entity.StreetId) && !_streetRepository.GetAll().Any(s => s.Id == entity.StreetId))
                throw new ArgumentException("La calle asociada no existe.");
            if (!string.IsNullOrEmpty(entity.BuildingCompanyId) && !_companyRepository.GetAll().Any(c => c.Id == entity.BuildingCompanyId))
                throw new ArgumentException("La empresa constructora asociada no existe.");
            if (!string.IsNullOrEmpty(entity.StatusId) && !_statusRepository.GetAll().Any(s => s.Id == entity.StatusId))
                throw new ArgumentException("El estado asociado no existe.");
            
            // Guardar el edificio primero
            _buildingRepository.Add(entity);
            
            // Crear automáticamente un registro de Address para el edificio
            // COMENTADO: Se maneja ahora desde el controller con datos del frontend
            // CreateAddressForBuilding(entity);
        }

        /// <summary>
        /// Agrega un nuevo edificio de manera asíncrona tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad Building a agregar.</param>
        public async Task AddAsync(Building entity)
        {
            // Sanitizar strings nulos/undefined a string vacío
            entity.Name = entity.Name ?? string.Empty;
            entity.Description = entity.Description ?? string.Empty;
            entity.Code = entity.Code ?? string.Empty;
            entity.Doorway = entity.Doorway ?? string.Empty;
            entity.DistrictId = entity.DistrictId ?? string.Empty;
            entity.StreetId = entity.StreetId ?? string.Empty;
            entity.BuildingCompanyId = entity.BuildingCompanyId ?? string.Empty;
            entity.StatusId = entity.StatusId ?? string.Empty;
            entity.EnergyCertificate = entity.EnergyCertificate ?? string.Empty;
            
            var all = await _buildingRepository.GetAllAsync();
            // Validación: Code único
            if (all.Any(b => b.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un edificio con ese código.");
            // Validación: Nombre único (solo si se proporciona un nombre)
            if (!string.IsNullOrEmpty(entity.Name) && all.Any(b => b.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un edificio con ese nombre.");
                
            var districts = await _districtRepository.GetAllAsync();
            // Validación: District, Street, Company y Status existen (solo si se proporcionan)
            if (!string.IsNullOrEmpty(entity.DistrictId) && !districts.Any(d => d.Id == entity.DistrictId))
                throw new ArgumentException("El distrito asociado no existe.");
            var streets = await _streetRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(entity.StreetId) && !streets.Any(s => s.Id == entity.StreetId))
                throw new ArgumentException("La calle asociada no existe.");
            var companies = await _companyRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(entity.BuildingCompanyId) && !companies.Any(c => c.Id == entity.BuildingCompanyId))
                throw new ArgumentException("La empresa constructora asociada no existe.");
            var statuses = await _statusRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(entity.StatusId) && !statuses.Any(s => s.Id == entity.StatusId))
                throw new ArgumentException("El estado asociado no existe.");

            await _buildingRepository.AddAsync(entity);
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
        /// Elimina un edificio por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador del edificio a eliminar.</param>
        public async Task DeleteAsync(string id)
        {
            var building = await _buildingRepository.GetByIdAsync(id);
            if (building == null)
                throw new ArgumentException("El edificio no existe.", nameof(id));
            await _buildingRepository.DeleteAsync(id);
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
        /// Obtiene todos los edificios de manera asíncrona.
        /// </summary>
        /// <returns>Una colección de edificios.</returns>
        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            return await _buildingRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtiene un edificio por su identificador.
        /// </summary>
        /// <param name="id">Identificador del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public Building? GetById(string id)
        {
            return _buildingRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene un edificio por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public async Task<Building?> GetByIdAsync(string id)
        {
            return await _buildingRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Obtiene un edificio por su código único.
        /// </summary>
        /// <param name="code">Código único del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public Building? GetByCode(string code)
        {
            // Llama al método específico del repositorio para obtener el edificio por código
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
                return repoImpl.GetBuildingByCode(code);
            throw new NotSupportedException("El repositorio no soporta GetBuildingByCode.");
        }

        /// <summary>
        /// Obtiene un edificio por su código único de manera asíncrona.
        /// </summary>
        /// <param name="code">Código único del edificio.</param>
        /// <returns>El edificio encontrado o null si no existe.</returns>
        public async Task<Building?> GetByCodeAsync(string code)
        {
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
            {
                // No hay versión asíncrona, así que usamos Task.Run
                return await Task.Run(() => repoImpl.GetBuildingByCode(code));
            }
            throw new NotSupportedException("El repositorio no soporta GetBuildingByCode.");
        }

        /// <summary>
        /// Actualiza un edificio existente.
        /// </summary>
        /// <param name="entity">Entidad Building a actualizar.</param>
        public void Update(Building entity)
        {
            // Sanitizar strings nulos/undefined a string vacío
            entity.Name = entity.Name ?? string.Empty;
            entity.Description = entity.Description ?? string.Empty;
            entity.Code = entity.Code ?? string.Empty;
            entity.Doorway = entity.Doorway ?? string.Empty;
            entity.DistrictId = entity.DistrictId ?? string.Empty;
            entity.StreetId = entity.StreetId ?? string.Empty;
            entity.BuildingCompanyId = entity.BuildingCompanyId ?? string.Empty;
            entity.StatusId = entity.StatusId ?? string.Empty;
            entity.EnergyCertificate = entity.EnergyCertificate ?? string.Empty;
            
            _buildingRepository.Update(entity);
        }

        /// <summary>
        /// Actualiza un edificio existente de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad Building a actualizar.</param>
        public async Task UpdateAsync(Building entity)
        {
            // Sanitizar strings nulos/undefined a string vacío
            entity.Name = entity.Name ?? string.Empty;
            entity.Description = entity.Description ?? string.Empty;
            entity.Code = entity.Code ?? string.Empty;
            entity.Doorway = entity.Doorway ?? string.Empty;
            entity.DistrictId = entity.DistrictId ?? string.Empty;
            entity.StreetId = entity.StreetId ?? string.Empty;
            entity.BuildingCompanyId = entity.BuildingCompanyId ?? string.Empty;
            entity.StatusId = entity.StatusId ?? string.Empty;
            entity.EnergyCertificate = entity.EnergyCertificate ?? string.Empty;
            
            await _buildingRepository.UpdateAsync(entity);
        }

        // Obtiene todos los edificios con Address y District incluidos
        public IEnumerable<Building> GetAllWithAddressAndDistrict()
        {
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
                return repoImpl.GetAllWithAddressAndDistrict();
            throw new NotSupportedException("El repositorio no soporta GetAllWithAddressAndDistrict.");
        }

        // Obtiene todos los edificios con Address y District incluidos de manera asíncrona
        public async Task<IEnumerable<Building>> GetAllWithAddressAndDistrictAsync()
        {
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
            {
                // No hay versión asíncrona, así que usamos Task.Run
                return await Task.Run(() => repoImpl.GetAllWithAddressAndDistrict());
            }
            throw new NotSupportedException("El repositorio no soporta GetAllWithAddressAndDistrict.");
        }

        // Obtiene un edificio por ID con Address y District incluidos
        public Building? GetByIdWithAddressAndDistrict(string id)
        {
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
                return repoImpl.GetByIdWithAddressAndDistrict(id);
            throw new NotSupportedException("El repositorio no soporta GetByIdWithAddressAndDistrict.");
        }

        // Obtiene un edificio por ID con Address y District incluidos de manera asíncrona
        public async Task<Building?> GetByIdWithAddressAndDistrictAsync(string id)
        {
            if (_buildingRepository is RepositoryLibraryProject.RepositoryIMP<Building> repoImpl)
            {
                // No hay versión asíncrona, así que usamos Task.Run
                return await Task.Run(() => repoImpl.GetByIdWithAddressAndDistrict(id));
            }
            throw new NotSupportedException("El repositorio no soporta GetByIdWithAddressAndDistrict.");
        }
    }
}
