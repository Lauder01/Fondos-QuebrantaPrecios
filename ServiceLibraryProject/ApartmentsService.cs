using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class ApartmentsService : IService<Apartment>
    {
        public void Add(Apartment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Apartment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Apartment? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Apartment entity)
        {
            throw new NotImplementedException();
        }
    }
}
