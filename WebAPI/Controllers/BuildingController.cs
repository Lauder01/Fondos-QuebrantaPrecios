using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.Building;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.BuildingService _buildingService;
        public BuildingController(ServiceLibraryProject.BuildingService buildingService, IMapper mapper)
        {
            _buildingService = buildingService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BuildingGetterDto>> GetAll()
        {
            var buildings = _buildingService.GetAll();
            var dtos = _mapper.Map<IEnumerable<BuildingGetterDto>>(buildings);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BuildingGetterDto> GetById(string id)
        {
            var building = _buildingService.GetById(id);
            if (building == null) return NotFound();
            var dto = _mapper.Map<BuildingGetterDto>(building);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<BuildingGetterDto> Create(BuildingCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var building = _mapper.Map<Building>(dto);
            building.Id = Guid.NewGuid().ToString();
            _buildingService.Add(building);
            var result = _mapper.Map<BuildingGetterDto>(building);
            return CreatedAtAction(nameof(GetById), new { id = building.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, BuildingUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var building = _mapper.Map<Building>(dto);
            building.Id = id;
            _buildingService.Update(building);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _buildingService.Delete(id);
            return NoContent();
        }
    }
}
