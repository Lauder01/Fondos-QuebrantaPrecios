using System;
using System.Collections.Generic;
using System.Linq;
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
                .ToList();
        }

        /// <summary>
        /// Obtiene un distrito por su identificador, incluyendo sus códigos postales asociados.
        /// </summary>
        /// <param name="id">Identificador del distrito.</param>
        /// <returns>El distrito encontrado o null si no existe.</returns>
        public District? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .FirstOrDefault(d => d.Id == id);
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
                .FirstOrDefault(d => d.Name == name);
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
                .FirstOrDefault(d => d.Zipcode.Any(z => z.Code == code));
        }

        /// <summary>
        /// Agrega un nuevo distrito tras validar sus datos.
        /// </summary>
        /// <param name="entity">Entidad District a agregar.</param>
        public void Add(District entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre del distrito es obligatorio y debe tener entre 2 y 255 caracteres.");
            if (_districtRepository.GetAll().Any(d => d.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un distrito con ese nombre.");
            if (entity.BuildingCount < 0)
                throw new ArgumentException("El número de edificios no puede ser negativo.");
            _districtRepository.Add(entity);
        }

        /// <summary>
        /// Actualiza un distrito existente.
        /// </summary>
        /// <param name="entity">Entidad District a actualizar.</param>
        public void Update(District entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El distrito no puede ser nulo.");
            _districtRepository.Update(entity);
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
    }
}
