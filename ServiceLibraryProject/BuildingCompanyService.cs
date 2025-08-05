using System;
using System.Collections.Generic;
using System.Linq;
using FQP.Entities;
using FQP.Service.Interfaces;
using FQP.Repository.Interfaces;

namespace FQP.Service
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
            // Validación: Nombre requerido y único
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre de la empresa es obligatorio y debe tener entre 2 y 255 caracteres.");
            if (_companyRepository.GetAll().Any(c => c.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una empresa con ese nombre.");
            // Validación: CIF requerido, longitud y único
            if (string.IsNullOrWhiteSpace(entity.Cif) || entity.Cif.Length != 9)
                throw new ArgumentException("El CIF es obligatorio y debe tener 9 caracteres.");
            if (_companyRepository.GetAll().Any(c => c.Cif == entity.Cif))
                throw new InvalidOperationException("Ya existe una empresa con ese CIF.");
            // Validación: Website longitud
            if (!string.IsNullOrEmpty(entity.Website) && entity.Website.Length > 1024)
                throw new ArgumentException("La web no puede superar los 1024 caracteres.");
            _companyRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BuildingCompany> GetAll()
        {
            throw new NotImplementedException();
        }

        public BuildingCompany? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(BuildingCompany entity)
        {
            throw new NotImplementedException();
        }
    }
}
