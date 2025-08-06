using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;

namespace RepositoryLibraryProject
{
    public class StreetRepository : IRepository<Street>
    {
        public void Add(Street entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Street> GetAll()
        {
            throw new NotImplementedException();
        }

        public Street? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Street entity)
        {
            throw new NotImplementedException();
        }
    }
}
