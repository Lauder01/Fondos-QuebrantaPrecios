using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;

namespace RepositoryLibraryProject
{
    public class StatusRepository : IRepository<Status>
    {
        public void Add(Status entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Status> GetAll()
        {
            throw new NotImplementedException();
        }

        public Status? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Status entity)
        {
            throw new NotImplementedException();
        }
    }
}
