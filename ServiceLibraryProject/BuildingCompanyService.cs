using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task AddAsync(BuildingCompany entity)
        {
            var all = await _companyRepository.GetAllAsync();
            if (all.Any(c => c.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una empresa con ese nombre.");

            if (all.Any(c => c.Cif == entity.Cif))
                throw new InvalidOperationException("Ya existe una empresa con ese CIF.");

            await _companyRepository.AddAsync(entity);
        }

        public void Delete(string id)
        {
            _ = _companyRepository.GetById(id) ?? throw new ArgumentException("La empresa no existe.", nameof(id));
            _companyRepository.Delete(id);
        }

        public async Task DeleteAsync(string id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                throw new ArgumentException("La empresa no existe.", nameof(id));
            await _companyRepository.DeleteAsync(id);
        }

        public IEnumerable<BuildingCompany> GetAll()
        {
            return _companyRepository.GetAll();
        }

        public async Task<IEnumerable<BuildingCompany>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public BuildingCompany? GetByCif(string cif)
        {
            if (string.IsNullOrWhiteSpace(cif))
                throw new ArgumentException("El CIF proporcionado no existe.", nameof(cif));
            return _companyRepository.Find(c => c.Cif == cif);
        }

        public async Task<BuildingCompany?> GetByCifAsync(string cif)
        {
            if (string.IsNullOrWhiteSpace(cif))
                throw new ArgumentException("El CIF proporcionado no existe.", nameof(cif));
            return await _companyRepository.FindAsync(c => c.Cif == cif);
        }

        public BuildingCompany? GetById(string id)
        {
            return _companyRepository.GetById(id);
        }

        public async Task<BuildingCompany?> GetByIdAsync(string id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }

        public void Update(BuildingCompany entity)
        {
            _companyRepository.Update(entity);
        }

        public async Task UpdateAsync(BuildingCompany entity)
        {
            await _companyRepository.UpdateAsync(entity);
        }
    }
}
