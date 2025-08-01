using System;
using System.Collections.Generic;
using FQP.Entities;
using FQP.Service.Interfaces;

namespace FQP.Service
{
    public class RequestService : IService<Request>
    {
        public void Add(Request entity) => throw new NotImplementedException();
        public void Delete(Guid id) => throw new NotImplementedException();
        public IEnumerable<Request> GetAll() => throw new NotImplementedException();
        public Request? GetById(Guid id) => throw new NotImplementedException();
        public void Update(Request entity) => throw new NotImplementedException();
    }
}
