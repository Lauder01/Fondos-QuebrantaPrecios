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
            _ = _apartmentRepository.GetById(id) ?? throw new ArgumentException("El apartamento no existe.", nameof(id));
            _apartmentRepository.Delete(id);
        }

        public IEnumerable<Apartment> GetAll()
        {
            return _apartmentRepository.GetAll();
        }

        public Apartment? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _apartmentRepository.GetById(id);
        }

        public Apartment? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _apartmentRepository.Find(a => a.Code == code);
        }

        public void Update(Apartment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El apartamento no puede ser nulo.");
            _apartmentRepository.Update(entity);
        }
    }
}
