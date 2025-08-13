using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;
using RepositoryLibraryProject.Interfaces;

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
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public T? Find(Func<T, bool> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }
    }
}
