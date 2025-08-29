using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar la entidad en la base de datos.", ex);
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

        public T? Find(Func<T, bool> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
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

        // Nuevo método: Obtener Building por Code
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
    }
}
