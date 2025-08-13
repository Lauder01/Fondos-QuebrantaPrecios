using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class ApartmentService : IService<Apartment>
    {
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<Floor> _floorRepository;
        private readonly IRepository<Building> _buildingRepository;

        public ApartmentService(IRepository<Apartment> apartmentRepository, IRepository<Floor> floorRepository, IRepository<Building> buildingRepository)
        {
            _apartmentRepository = apartmentRepository;
            _floorRepository = floorRepository;
            _buildingRepository = buildingRepository;
        }

        public void Add(Apartment entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Code) || entity.Code.Length > 50)
                throw new ArgumentException("El código del apartamento es obligatorio y debe tener como máximo 50 caracteres.");
            if (_apartmentRepository.GetAll().Any(a => a.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un apartamento con ese código.");

            if (string.IsNullOrWhiteSpace(entity.Door) || entity.Door.Length > 24)
                throw new ArgumentException("La puerta es obligatoria y debe tener como máximo 24 caracteres.");

            if (entity.Floor == null || !_floorRepository.GetAll().Any(f => f.Id == entity.Floor.Id))
                throw new ArgumentException("El piso asociado no existe.");
            _apartmentRepository.Add(entity);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Apartment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Apartment? GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Apartment entity)
        {
            throw new NotImplementedException();
        }
    }
}
