using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class BuildingCompanyService : IService<BuildingCompany>
    {
        private readonly IRepository<BuildingCompany> _companyRepository;

        public BuildingCompanyService(IRepository<BuildingCompany> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public void Add(BuildingCompany entity)
        {
            if (_companyRepository.GetAll().Any(c => c.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una empresa con ese nombre.");

            if (_companyRepository.GetAll().Any(c => c.Cif == entity.Cif))
                throw new InvalidOperationException("Ya existe una empresa con ese CIF.");

            _companyRepository.Add(entity);
        }

        public void Delete(string id)
        {
            _ = _companyRepository.GetById(id) ?? throw new ArgumentException("La empresa no existe.", nameof(id));
            _companyRepository.Delete(id);
        }

        public IEnumerable<BuildingCompany> GetAll()
        {
            return _companyRepository.GetAll();
        }

        public BuildingCompany? GetByCif(string cif)
        {
            if (string.IsNullOrWhiteSpace(cif))
                throw new ArgumentException("El CIF proporcionado no existe.", nameof(cif));
            return _companyRepository.Find(c => c.Cif == cif);
        }

        public BuildingCompany? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _companyRepository.GetById(id);
        }

        public void Update(BuildingCompany entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La empresa no puede ser nula.");
            _companyRepository.Update(entity);
        }
    }
}
