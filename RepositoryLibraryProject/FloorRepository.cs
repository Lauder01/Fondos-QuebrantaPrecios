using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;

namespace RepositoryLibraryProject
{
    public class FloorRepository : IRepository<Floor>
    {
        public void Add(Floor entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Floor> GetAll()
        {
            throw new NotImplementedException();
        }

        public Floor? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Floor entity)
        {
            throw new NotImplementedException();
        }
    }
}
