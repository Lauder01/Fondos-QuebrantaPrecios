using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FQP.Entities;
using FQP.Repository.Interfaces;

namespace FQP.Repository
{
    public class CompanyRepository : IRepository<Company>
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
