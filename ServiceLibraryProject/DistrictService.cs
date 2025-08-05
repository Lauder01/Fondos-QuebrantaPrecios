using System;
using System.Collections.Generic;
using System.Linq;
using FQP.Entities;
using FQP.Service.Interfaces;
using FQP.Repository.Interfaces;

namespace FQP.Service
{
    public class DistrictService : IService<District>
    {
        private readonly IRepository<District> _districtRepository;

        public DistrictService(IRepository<District> districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public void Add(District entity)
        {
            // Validación: Nombre requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre del distrito es obligatorio y debe tener entre 2 y 255 caracteres.");
            // Validación: ZipCode requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.ZipCode) || entity.ZipCode.Length < 2 || entity.ZipCode.Length > 255)
                throw new ArgumentException("El código postal es obligatorio y debe tener entre 2 y 255 caracteres.");
            // Validación: Unicidad de nombre
            if (_districtRepository.GetAll().Any(d => d.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un distrito con ese nombre.");
            // Validación: Unicidad de ZipCode
            if (_districtRepository.GetAll().Any(d => d.ZipCode == entity.ZipCode))
                throw new InvalidOperationException("Ya existe un distrito con ese código postal.");
            // Validación: BuildingCount >= 0
            if (entity.BuildingCount < 0)
                throw new ArgumentException("El número de edificios no puede ser negativo.");
            _districtRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<District> GetAll()
        {
            throw new NotImplementedException();
        }

        public District? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(District entity)
        {
            throw new NotImplementedException();
        }
    }
}
