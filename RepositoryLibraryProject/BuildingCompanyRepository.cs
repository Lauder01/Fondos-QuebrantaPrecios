using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;

namespace RepositoryLibraryProject
{
    public class BuildingCompanyRepository : IRepository<BuildingCompany>
    {
        public void Add(BuildingCompany entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BuildingCompany> GetAll()
        {
            throw new NotImplementedException();
        }

        public BuildingCompany? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(BuildingCompany entity)
        {
            throw new NotImplementedException();
        }
    }
}
