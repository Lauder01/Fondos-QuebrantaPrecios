using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;

namespace RepositoryLibraryProject
{
    public class DistrictRepository : IRepository<District>
    {
        public void Add(District entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<District> GetAll()
        {
            throw new NotImplementedException();
        }

        public District? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(District entity)
        {
            throw new NotImplementedException();
        }
    }
}
