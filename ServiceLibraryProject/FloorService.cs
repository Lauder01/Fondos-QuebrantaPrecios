using System;
using System.Collections.Generic;
using System.Linq;
using FQP.Entities;
using FQP.Service.Interfaces;
using FQP.Repository.Interfaces;

namespace FQP.Service
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
            if (entity.BuildingFloor == null || !_buildingRepository.GetAll().Any(b => b.Id == entity.BuildingFloor.Id))
                throw new ArgumentException("El edificio asociado no existe.");
            _floorRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Floor> GetAll()
        {
            throw new NotImplementedException();
        }

        public Floor? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Floor entity)
        {
            throw new NotImplementedException();
        }
    }
}
