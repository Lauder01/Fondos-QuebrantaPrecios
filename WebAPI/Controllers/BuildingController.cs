using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IRepository<Building> _buildingRepository;
        private readonly IMapper _mapper;
        public BuildingController(IRepository<Building> buildingRepository, IMapper mapper)
        {
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BuildingDto>> GetAll()
        {
            var buildings = _buildingRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<BuildingDto>>(buildings);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<BuildingDto> Create(CreateBuildingDto dto)
        {
            var building = _mapper.Map<Building>(dto);
            building.Id = Guid.NewGuid().ToString();
            _buildingRepository.Add(building);
            var result = _mapper.Map<BuildingDto>(building);
            return CreatedAtAction(nameof(GetAll), new { id = building.Id }, result);
        }
    }
}
