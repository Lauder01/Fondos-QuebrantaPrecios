using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;
using RepositoryLibraryProject.Interfaces;
using ClassLibraryProject.Entities;

namespace RepositoryLibraryProject
{
    public class RepositoryIMP<T>(AppDbContext context) : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener los datos de la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener los datos de la base de datos (async).", ex);
            }
        }

        public T? GetById(string id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener la entidad por ID.", ex);
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener la entidad por ID (async).", ex);
            }
        }

        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar la entidad a la base de datos.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar la entidad a la base de datos (async).", ex);
            }
        }

        public void Update(T entity)
        {
            try
            {
                // Obtener el valor de la clave primaria
                var keyProperty = typeof(T).GetProperty("Id");
                if (keyProperty == null)
                    throw new InvalidOperationException("La entidad debe tener una propiedad 'Id'.");
                
                var keyValue = keyProperty.GetValue(entity)?.ToString();
                if (string.IsNullOrEmpty(keyValue))
                    throw new InvalidOperationException("El valor del Id no puede ser nulo o vacío.");

                // Buscar si ya existe una entidad tracked con el mismo ID
                var trackedEntity = _context.Entry(entity).Entity;
                var existingEntry = _context.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e => keyProperty.GetValue(e.Entity)?.ToString() == keyValue);

                if (existingEntry != null)
                {
                    // Si ya existe una entidad tracked, actualizar sus valores
                    existingEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    // Si no existe, usar Update normal
                    _dbSet.Update(entity);
                }
                
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar la entidad en la base de datos.", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                // Obtener el valor de la clave primaria
                var keyProperty = typeof(T).GetProperty("Id");
                if (keyProperty == null)
                    throw new InvalidOperationException("La entidad debe tener una propiedad 'Id'.");
                
                var keyValue = keyProperty.GetValue(entity)?.ToString();
                if (string.IsNullOrEmpty(keyValue))
                    throw new InvalidOperationException("El valor del Id no puede ser nulo o vacío.");

                // Buscar si ya existe una entidad tracked con el mismo ID
                var trackedEntity = _context.Entry(entity).Entity;
                var existingEntry = _context.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e => keyProperty.GetValue(e.Entity)?.ToString() == keyValue);

                if (existingEntry != null)
                {
                    // Si ya existe una entidad tracked, actualizar sus valores
                    existingEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    // Si no existe, usar Update normal
                    _dbSet.Update(entity);
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar la entidad en la base de datos (async).", ex);
            }
        }

        public void Delete(string id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al eliminar la entidad de la base de datos.", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al eliminar la entidad de la base de datos (async).", ex);
            }
        }

        public T? Find(Func<T, bool> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public Task<T?> FindAsync(Func<T, bool> predicate)
        {
            // No hay equivalente asíncrono directo para FirstOrDefault con predicado en memoria
            // Se recomienda usar expresiones para consultas a BD, pero aquí mantenemos la firma
            return Task.FromResult(_dbSet.AsNoTracking().FirstOrDefault(predicate));
        }

        // Métodos específicos para Building con Address y District
        public IEnumerable<Building> GetAllWithAddressAndDistrict()
        {
            if (typeof(T) == typeof(Building))
                return _context.Set<Building>()
                    .Include(b => b.Address)
                    .Include(b => b.District)
                    .AsNoTracking()
                    .ToList() as IEnumerable<Building>;
            throw new NotSupportedException("GetAllWithAddressAndDistrict solo es válido para Building.");
        }

        public Building? GetByIdWithAddressAndDistrict(string id)
        {
            if (typeof(T) == typeof(Building))
                return _context.Set<Building>()
                    .Include(b => b.Address)
                    .Include(b => b.District)
                    .AsNoTracking()
                    .FirstOrDefault(b => b.Id == id) as Building;
            throw new NotSupportedException("GetByIdWithAddressAndDistrict solo es válido para Building.");
        }

        // Nuevo método: Obtener Building por Code. 
        // Este método lo usamos para SpecuLab.
        public Building? GetBuildingByCode(string code)
        {
            if (typeof(T) == typeof(Building))
                return _context.Set<Building>()
                    .Include(b => b.Address)
                    .Include(b => b.District)
                    .AsNoTracking()
                    .FirstOrDefault(b => b.Code == code) as Building;
            throw new NotSupportedException("GetBuildingByCode solo es válido para Building.");
        }

        //Método para mandar request a SpecuLab
        public Request? PostRequest(Request request)
        {
            try
            {
                _context.Request.Add(request);
                _context.SaveChanges();
                return request;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar la solicitud a la base de datos.", ex);
            }
        }
    }
}
