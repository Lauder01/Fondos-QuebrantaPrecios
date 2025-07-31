using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FQP.Entities;
using FQP.Service.Interfaces;

namespace FQP.Service
{
    public class CompanyService : IService<Company>
    {
        public void Add(Company entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Company> GetAll()
        {
            throw new NotImplementedException();
        }

        public Company? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}
