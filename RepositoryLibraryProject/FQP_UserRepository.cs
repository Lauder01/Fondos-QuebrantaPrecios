using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FQP.Entities;
using FQP.Repository.Interfaces;

namespace FQP.Repository
{
    public class FQP_UserRepository : IRepository<FQP_User>
    {
        public void Add(FQP_User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FQP_User> GetAll()
        {
            throw new NotImplementedException();
        }

        public FQP_User? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(FQP_User entity)
        {
            throw new NotImplementedException();
        }
    }
}
