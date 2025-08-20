using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;

namespace ServiceLibraryProject
{
    public class DistrictService(IRepository<District> districtRepository, AppDbContext context) : IService<District>
    {
        private readonly IRepository<District> _districtRepository = districtRepository;
        private readonly AppDbContext _context = context;

        public IEnumerable<District> GetAll()
        {
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .ToList();
        }

        public District? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Id == id);
        }

        public District? GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Name == name);
        }

        public District? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            // Buscar por el primer código postal asociado
            return _context.Set<District>()
                .Include(d => d.Zipcode)
                .Include(d => d.Street)
                .FirstOrDefault(d => d.Zipcode.Any(z => z.Code == code));
        }

        public void Add(District entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre del distrito es obligatorio y debe tener entre 2 y 255 caracteres.");

            // No validamos ZipCode aquí, ya que ahora es una colección

            if (_districtRepository.GetAll().Any(d => d.Name == entity.Name))
                throw new InvalidOperationException("Ya existe un distrito con ese nombre.");

            // No validamos duplicados de ZipCode aquí, ya que ahora es una colección

            if (entity.BuildingCount < 0)
                throw new ArgumentException("El número de edificios no puede ser negativo.");

            _districtRepository.Add(entity);
        }

        public void Update(District entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El distrito no puede ser nulo.");

            _districtRepository.Update(entity);
        }

        public void Delete(string id)
        {
            _ = _districtRepository.GetById(id) ?? throw new ArgumentException("El distrito no existe.", nameof(id));
            _districtRepository.Delete(id);
        }
    }
}
