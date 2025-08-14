using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class AdressService : IService<Address>
    {
        private readonly IRepository<Address> _addressRepository;

        public AdressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public IEnumerable<Address> GetAll()
        {
            return _addressRepository.GetAll();
        }

        public Address? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _addressRepository.GetById(id);
        }

        public void Add(Address entity)
        {
            _addressRepository.Add(entity);
        }

        public void Update(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La dirección no puede ser nula.");
            _addressRepository.Update(entity);
        }

        public void Delete(string id)
        {
            _ = _addressRepository.GetById(id) ?? throw new ArgumentException("La dirección no existe.", nameof(id));
            _addressRepository.Delete(id);
        }
    }
}
