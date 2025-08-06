using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class StatusService : IService<Status>
    {
        private readonly IRepository<Status> _statusRepository;

        public StatusService(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public void Add(Status entity)
        {
            // Validación: Nombre requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length > 48)
                throw new ArgumentException("El nombre del estado es obligatorio y debe tener como máximo 48 caracteres.");
            // Validación: Unicidad de nombre
            if (_statusRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un estado con ese nombre.");
            _statusRepository.Add(entity);
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
