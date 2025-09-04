using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class ZipcodeService : IService<Zipcode>
    {
        private readonly IRepository<Zipcode> _zipcodeRepository;

        public ZipcodeService(IRepository<Zipcode> zipcodeRepository)
        {
            _zipcodeRepository = zipcodeRepository;
        }

        public IEnumerable<Zipcode> GetAll()
        {
            return _zipcodeRepository.GetAll();
        }

        public async Task<IEnumerable<Zipcode>> GetAllAsync()
        {
            return await _zipcodeRepository.GetAllAsync();
        }

        public Zipcode? GetById(string id)
        {
            return _zipcodeRepository.GetById(id);
        }

        public async Task<Zipcode?> GetByIdAsync(string id)
        {
            return await _zipcodeRepository.GetByIdAsync(id);
        }

        public Zipcode? GetByCode(string code)
        {
            return _zipcodeRepository.Find(z => z.Code == code);
        }

        public async Task<Zipcode?> GetByCodeAsync(string code)
        {
            return await _zipcodeRepository.FindAsync(z => z.Code == code);
        }

        public void Add(Zipcode entity)
        {
            if (_zipcodeRepository.GetAll().Any(z => z.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un código postal con ese código.");
            _zipcodeRepository.Add(entity);
        }

        public async Task AddAsync(Zipcode entity)
        {
            var all = await _zipcodeRepository.GetAllAsync();
            if (all.Any(z => z.Code == entity.Code))
                throw new InvalidOperationException("Ya existe un código postal con ese código.");
            await _zipcodeRepository.AddAsync(entity);
        }

        public void Update(Zipcode entity)
        {
            _zipcodeRepository.Update(entity);
        }

        public async Task UpdateAsync(Zipcode entity)
        {
            await _zipcodeRepository.UpdateAsync(entity);
        }

        public void Delete(string id)
        {
            _ = _zipcodeRepository.GetById(id) ?? throw new ArgumentException("El código postal no existe.", nameof(id));
            _zipcodeRepository.Delete(id);
        }

        public async Task DeleteAsync(string id)
        {
            var zipcode = await _zipcodeRepository.GetByIdAsync(id);
            if (zipcode == null)
                throw new ArgumentException("El código postal no existe.", nameof(id));
            await _zipcodeRepository.DeleteAsync(id);
        }
    }
}
