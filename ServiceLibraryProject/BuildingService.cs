using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class BuildingService(
        IRepository<Building> buildingRepository,
        IRepository<District> districtRepository,
        IRepository<Street> streetRepository,
        IRepository<BuildingCompany> companyRepository,
        IRepository<Status> statusRepository) : IService<Building>
    {
        private readonly IRepository<Building> _buildingRepository = buildingRepository;
        private readonly IRepository<District> _districtRepository = districtRepository;
        private readonly IRepository<Street> _streetRepository = streetRepository;
        private readonly IRepository<BuildingCompany> _companyRepository = companyRepository;
        private readonly IRepository<Status> _statusRepository = statusRepository;

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
            if (entity.District == null || !_districtRepository.GetAll().Any(d => d.Id == entity.District.Id))
                throw new ArgumentException("El distrito asociado no existe.");
            if (entity.Street == null || !_streetRepository.GetAll().Any(s => s.Id == entity.Street.Id))
                throw new ArgumentException("La calle asociada no existe.");
            if (entity.BuildingCompany == null || !_companyRepository.GetAll().Any(c => c.Id == entity.BuildingCompany.Id))
                throw new ArgumentException("La empresa constructora asociada no existe.");
            // Reemplaza la validación de Status por BuildingStatus, que es la propiedad correcta según la definición de Building.
            if (entity.Status == null || !_statusRepository.GetAll().Any(s => s.Id == entity.Status.Id))
                throw new ArgumentException("El estado asociado no existe.");
            _buildingRepository.Add(entity);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Building> GetAll()
        {
            throw new NotImplementedException();
        }

        public Building? GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Building entity)
        {
            throw new NotImplementedException();
        }
    }
}
