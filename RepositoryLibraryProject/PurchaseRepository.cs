using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FQP.Entities;
using FQP.Repository.Interfaces;

namespace FQP.Repository
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        public void Add(Purchase entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> GetAll()
        {
            throw new NotImplementedException();
        }

        public Purchase? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Purchase entity)
        {
            throw new NotImplementedException();
        }
    }
}
