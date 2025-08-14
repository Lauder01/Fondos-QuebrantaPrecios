using System;
using System.Collections.Generic;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class RequestService : IService<Request>
    {
        private readonly IRepository<Request> _requestRepository;

        public RequestService(IRepository<Request> requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public IEnumerable<Request> GetAll()
        {
            return _requestRepository.GetAll();
        }

        public Request? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _requestRepository.GetById(id);
        }

        public void Add(Request entity)
        {
            _requestRepository.Add(entity);
        }

        public void Update(Request entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La solicitud no puede ser nula.");
            _requestRepository.Update(entity);
        }

        public void Delete(string id)
        {
            _ = _requestRepository.GetById(id) ?? throw new ArgumentException("La solicitud no existe.", nameof(id));
            _requestRepository.Delete(id);
        }
    }
}
