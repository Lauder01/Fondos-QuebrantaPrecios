using System;
using System.Collections.Generic;

namespace ServiceLibraryProject.Interfaces
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(string id);
    }
}
