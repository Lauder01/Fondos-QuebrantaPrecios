using System;
using System.Collections.Generic;
using System.Linq;
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
            // Validación opcional: evitar duplicados por FileName y BuildingId
            if (_buildingImageRepository.GetAll().Any(bi => bi.FileName == entity.FileName && bi.BuildingId == entity.BuildingId))
                throw new InvalidOperationException("Ya existe una imagen con ese nombre para el edificio.");

            _buildingImageRepository.Add(entity);
        }

        public void Delete(string id)
        {
            _ = _buildingImageRepository.GetById(id) ?? throw new ArgumentException("La imagen no existe.", nameof(id));
            _buildingImageRepository.Delete(id);
        }

        public IEnumerable<BuildingImage> GetAll()
        {
            return _buildingImageRepository.GetAll();
        }

        public BuildingImage? GetById(string id)
        {
            return _buildingImageRepository.GetById(id);
        }

        public IEnumerable<BuildingImage> GetByBuildingId(string buildingId)
        {
            return _buildingImageRepository.GetAll().Where(bi => bi.BuildingId == buildingId);
        }

        public void Update(BuildingImage entity)
        {
            _buildingImageRepository.Update(entity);
        }
    }
}
