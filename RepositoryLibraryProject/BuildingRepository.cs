using F.Entities;
using FQP.Entities;
using RepositoryLibraryProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQP.Repository
{
    public class BuildingRepository : IRepository<Building>
    {
        public void Add(Building entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Building> GetAll()
        {
            throw new NotImplementedException();
        }

        public Building? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Building entity)
        {
            throw new NotImplementedException();
        }
    }
}
