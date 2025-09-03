using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLibraryProject.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(string id);
        T? Find(Func<T, bool> predicate);

        // Métodos asíncronos
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<T?> FindAsync(Func<T, bool> predicate);
    }
}
