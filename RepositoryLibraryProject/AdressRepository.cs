
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FQP.Entities;
using FQP.Repository.Interfaces;

namespace FQP.Repository
{
    public class AdressRepository : IRepository<Address>
    {
        public void Add(Address entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Address> GetAll()
        {
            throw new NotImplementedException();
        }

        public Address? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
