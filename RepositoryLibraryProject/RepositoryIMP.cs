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

        // Métodos específicos para Building con Address
        public IEnumerable<Building> GetAllWithAddress()
        {
            if (typeof(T) == typeof(Building))
                return _context.Set<Building>().Include(b => b.Address).AsNoTracking().ToList() as IEnumerable<Building>;
            throw new NotSupportedException("GetAllWithAddress solo es válido para Building.");
        }

        public Building? GetByIdWithAddress(string id)
        {
            if (typeof(T) == typeof(Building))
                return _context.Set<Building>().Include(b => b.Address).AsNoTracking().FirstOrDefault(b => b.Id == id) as Building;
            throw new NotSupportedException("GetByIdWithAddress solo es válido para Building.");
        }
    }
}
