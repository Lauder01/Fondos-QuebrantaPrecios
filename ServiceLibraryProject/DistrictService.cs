using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con distritos (District).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar distritos en la base de datos, incluyendo la carga de códigos postales asociados.
    /// </summary>
    public class DistrictService : IService<District>
    {
        /// <summary>
        /// Repositorio de distritos utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<District> _districtRepository;
        /// <summary>
        /// Contexto de base de datos para operaciones avanzadas con Entity Framework.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de distritos.
        /// </summary>
        /// <param name="districtRepository">Repositorio de distritos.</param>
        /// <param name="context">Contexto de base de datos.</param>
        public DistrictService(IRepository<District> districtRepository, AppDbContext context)
        {
            _districtRepository = districtRepository;
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los distritos, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <returns>Una colección de distritos.</returns>
        public IEnumerable<District> GetAll()
        {
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .ToList();
        }

        /// <summary>
        /// Obtiene todos los distritos de manera asíncrona, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona, con una colección de distritos como resultado.</returns>
        public async Task<IEnumerable<District>> GetAllAsync()
        {
            return await _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un distrito por su identificador, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <param name="id">Identificador del distrito.</param>
        /// <returns>El distrito encontrado o null si no existe.</returns>
        public District? GetById(string id)
        {
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// Obtiene un distrito por su identificador de manera asíncrona, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <param name="id">Identificador del distrito.</param>
        /// <returns>Una tarea que representa la operación asíncrona, con el distrito encontrado como resultado.</returns>
        public async Task<District?> GetByIdAsync(string id)
        {
            return await _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// Obtiene un distrito por su nombre, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <param name="name">Nombre del distrito.</param>
        /// <returns>El distrito encontrado o null si no existe.</returns>
        public District? GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Name == name);
        }

        /// <summary>
        /// Obtiene un distrito por su nombre de manera asíncrona, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <param name="name">Nombre del distrito.</param>
        /// <returns>Una tarea que representa la operación asíncrona, con el distrito encontrado como resultado.</returns>
        public async Task<District?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return await _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        /// <summary>
        /// Obtiene un distrito por uno de sus códigos postales asociados.
        /// </summary>
        /// <param name="code">Código postal.</param>
        /// <returns>El distrito encontrado o null si no existe.</returns>
        public District? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Zipcode.Any(z => z.Code == code));
        }

        /// <summary>
        /// Obtiene un distrito por uno de sus códigos postales asociados de manera asíncrona.
        /// </summary>
        /// <param name="code">Código postal.</param>
        /// <returns>Una tarea que representa la operación asíncrona, con el distrito encontrado como resultado.</returns>
        public async Task<District?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return await _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefaultAsync(d => d.Zipcode.Any(z => z.Code == code));
        }

        /// <summary>
        /// Agrega un nuevo distrito tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad District a agregar.</param>
        public void Add(District entity)
        {

            if (_districtRepository.GetAll().Any(d => d.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un distrito con ese nombre.");
            _districtRepository.Add(entity);
        }

        /// <summary>
        /// Agrega un nuevo distrito de manera asíncrona tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad District a agregar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(District entity)
        {
            var all = await _districtRepository.GetAllAsync();
            if (all.Any(d => d.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un distrito con ese nombre.");
            await _districtRepository.AddAsync(entity);
        }

        /// <summary>
        /// Actualiza un distrito existente.
        /// </summary>
        /// <param name="entity">Entidad District a actualizar.</param>
        public void Update(District entity)
        {
            _districtRepository.Update(entity);
        }

        /// <summary>
        /// Actualiza un distrito existente de manera asíncrona.
        /// </summary>
        /// <param name="entity">Entidad District a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task UpdateAsync(District entity)
        {
            await _districtRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Elimina un distrito por su identificador.
        /// </summary>
        /// <param name="id">Identificador del distrito a eliminar.</param>
        public void Delete(string id)
        {
            _ = _districtRepository.GetById(id) ?? throw new ArgumentException("El distrito no existe.", nameof(id));
            _districtRepository.Delete(id);
        }

        /// <summary>
        /// Elimina un distrito por su identificador de manera asíncrona.
        /// </summary>
        /// <param name="id">Identificador del distrito a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task DeleteAsync(string id)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            if (district == null)
                throw new ArgumentException("El distrito no existe.", nameof(id));
            await _districtRepository.DeleteAsync(id);
        }
    }
}
