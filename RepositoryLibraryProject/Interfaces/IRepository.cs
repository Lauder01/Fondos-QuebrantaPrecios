using System;
using System.Collections.Generic;

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
    }
}
