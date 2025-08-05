using System;
using System.Collections.Generic;
using System.Linq;
using FQP.Entities;
using FQP.Service.Interfaces;
using FQP.Repository.Interfaces;

namespace FQP.Service
{
    public class BuildingService : IService<Building>
    {
        private readonly IRepository<Building> _buildingRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Street> _streetRepository;
        private readonly IRepository<BuildingCompany> _companyRepository;
        private readonly IRepository<Status> _statusRepository;

        public BuildingService(
            IRepository<Building> buildingRepository,
            IRepository<District> districtRepository,
            IRepository<Street> streetRepository,
            IRepository<BuildingCompany> companyRepository,
            IRepository<Status> statusRepository)
        {
            _buildingRepository = buildingRepository;
            _districtRepository = districtRepository;
            _streetRepository = streetRepository;
            _companyRepository = companyRepository;
            _statusRepository = statusRepository;
        }

        public void Add(Building entity)
        {
            // Validación: Code único
            if (_buildingRepository.GetAll().Any(b => b.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un edificio con ese código.");
            // Validación: Doorway requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Doorway) || entity.Doorway.Length > 6)
                throw new ArgumentException("El portal es obligatorio y debe tener como máximo 6 caracteres.");
            // Validación: FloorCount >= 0
            if (entity.FloorCount < 0)
                throw new ArgumentException("El número de plantas no puede ser negativo.");
            // Validación: YearBuilt >= 1800 y <= año actual
            var year = entity.YearBuilt;
            var currentYear = DateTime.Now.Year;
            if (year < 1800 || year > currentYear)
                throw new ArgumentException($"El año de construcción debe estar entre 1800 y {currentYear}.");
            // Validación: Price >= 0
            if (entity.Price < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            // Validación: District, Street, Company y Status existen
            if (entity.BuildingDistrict == null || !_districtRepository.GetAll().Any(d => d.Id == entity.BuildingDistrict.Id))
                throw new ArgumentException("El distrito asociado no existe.");
            if (entity.BuildingStreet == null || !_streetRepository.GetAll().Any(s => s.Id == entity.BuildingStreet.Id))
                throw new ArgumentException("La calle asociada no existe.");
            if (entity.BuildingCompany == null || !_companyRepository.GetAll().Any(c => c.Id == entity.BuildingCompany.Id))
                throw new ArgumentException("La empresa constructora asociada no existe.");
            if (entity.Status == null || !_statusRepository.GetAll().Any(s => s.Id == entity.Status.Id))
                throw new ArgumentException("El estado asociado no existe.");
            _buildingRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Building> GetAll()
        {
            throw new NotImplementedException();
        }

        public Building? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Building entity)
        {
            throw new NotImplementedException();
        }
    }
}
