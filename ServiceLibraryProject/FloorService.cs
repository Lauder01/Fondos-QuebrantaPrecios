using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class FloorService : IService<Floor>
    {
        private readonly IRepository<Floor> _floorRepository;
        private readonly IRepository<Building> _buildingRepository;

        public FloorService(IRepository<Floor> floorRepository, IRepository<Building> buildingRepository)
        {
            _floorRepository = floorRepository;
            _buildingRepository = buildingRepository;
        }

        public void Add(Floor entity)
        {
            // Validación: FloorNumber >= -12
            if (entity.FloorNumber < -12)
                throw new ArgumentException("El número de piso no puede ser menor que -12.");
            // Validación: HasLift 0 o 1 (bool en C#)
            // Validación: BuildingId requerido y existencia
            if (entity.Building == null || !_buildingRepository.GetAll().Any(b => b.Id == entity.Building.Id))
                throw new ArgumentException("El edificio asociado no existe.");
            _floorRepository.Add(entity);
        }

        public void Delete(string id)
        {
            _ = _floorRepository.GetById(id) ?? throw new ArgumentException("El piso no existe.", nameof(id));
            _floorRepository.Delete(id);
        }

        public IEnumerable<Floor> GetAll()
        {
            return _floorRepository.GetAll();
        }

        public Floor? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _floorRepository.GetById(id);
        }

        public void Update(Floor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El piso no puede ser nulo.");
            _floorRepository.Update(entity);
        }
    }
}
