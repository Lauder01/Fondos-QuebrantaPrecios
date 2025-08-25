using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class ZipcodeService(IRepository<Zipcode> zipcodeRepository) : IService<Zipcode>
    {
        private readonly IRepository<Zipcode> _zipcodeRepository = zipcodeRepository;

        public IEnumerable<Zipcode> GetAll()
        {
            return _zipcodeRepository.GetAll();
        }

        public Zipcode? GetById(string id)
        {
            return _zipcodeRepository.GetById(id);
        }

        public Zipcode? GetByCode(string code)
        {
            return _zipcodeRepository.Find(z => z.Code == code);
        }

        public void Add(Zipcode entity)
        {
            if (_zipcodeRepository.GetAll().Any(z => z.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un código postal con ese código.");

            _zipcodeRepository.Add(entity);
        }

        public void Update(Zipcode entity)
        {
            _zipcodeRepository.Update(entity);
        }

        public void Delete(string id)
        {
            _ = _zipcodeRepository.GetById(id) ?? throw new ArgumentException("El código postal no existe.", nameof(id));
            _zipcodeRepository.Delete(id);
        }
    }
}
