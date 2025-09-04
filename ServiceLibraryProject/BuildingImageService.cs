using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    public class BuildingImageService : IService<BuildingImage>
    {
        private readonly IRepository<BuildingImage> _buildingImageRepository;

        public BuildingImageService(IRepository<BuildingImage> buildingImageRepository)
        {
            _buildingImageRepository = buildingImageRepository;
        }

        public void Add(BuildingImage entity)
        {
            if (_buildingImageRepository.GetAll().Any(bi => bi.FileName == entity.FileName && bi.BuildingId == entity.BuildingId))
                throw new InvalidOperationException("Ya existe una imagen con ese nombre para el edificio.");
            _buildingImageRepository.Add(entity);
        }

        public async Task AddAsync(BuildingImage entity)
        {
            var all = await _buildingImageRepository.GetAllAsync();
            if (all.Any(bi => bi.FileName == entity.FileName && bi.BuildingId == entity.BuildingId))
                throw new InvalidOperationException("Ya existe una imagen con ese nombre para el edificio.");
            await _buildingImageRepository.AddAsync(entity);
        }

        public void Delete(string id)
        {
            _ = _buildingImageRepository.GetById(id) ?? throw new ArgumentException("La imagen no existe.", nameof(id));
            _buildingImageRepository.Delete(id);
        }

        public async Task DeleteAsync(string id)
        {
            var image = await _buildingImageRepository.GetByIdAsync(id);
            if (image == null)
                throw new ArgumentException("La imagen no existe.", nameof(id));
            await _buildingImageRepository.DeleteAsync(id);
        }

        public IEnumerable<BuildingImage> GetAll()
        {
            return _buildingImageRepository.GetAll();
        }

        public async Task<IEnumerable<BuildingImage>> GetAllAsync()
        {
            return await _buildingImageRepository.GetAllAsync();
        }

        public BuildingImage? GetById(string id)
        {
            return _buildingImageRepository.GetById(id);
        }

        public async Task<BuildingImage?> GetByIdAsync(string id)
        {
            return await _buildingImageRepository.GetByIdAsync(id);
        }

        public IEnumerable<BuildingImage> GetByBuildingId(string buildingId)
        {
            return _buildingImageRepository.GetAll().Where(bi => bi.BuildingId == buildingId);
        }

        public async Task<IEnumerable<BuildingImage>> GetByBuildingIdAsync(string buildingId)
        {
            var all = await _buildingImageRepository.GetAllAsync();
            return all.Where(bi => bi.BuildingId == buildingId);
        }

        public void Update(BuildingImage entity)
        {
            _buildingImageRepository.Update(entity);
        }

        public async Task UpdateAsync(BuildingImage entity)
        {
            await _buildingImageRepository.UpdateAsync(entity);
        }
    }
}
