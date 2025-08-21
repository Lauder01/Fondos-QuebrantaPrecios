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
        /// <summary>
        /// Obtiene una lista paginada y filtrada de edificios.
        /// </summary>
        /// <param name="page">Página actual (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
        /// <param name="name">Filtro por nombre</param>
        /// <param name="districtId">Filtro por distrito</param>
        /// <param name="companyId">Filtro por empresa constructora</param>
        /// <returns>Lista paginada de edificios</returns>
        [HttpGet("paged")]
        public ActionResult<WebAPI.Dtos.Building.BuildingListResultDto> GetPaged(
            int page = 1,
            int pageSize = 10,
            string? name = null,
            string? districtId = null,
            string? companyId = null)
        {
            var result = _buildingService.GetPagedAndFiltered(page, pageSize, name, districtId, companyId);
            var dtos = _mapper.Map<IEnumerable<BuildingGetterDto>>(result.Items);
            return Ok(new WebAPI.Dtos.Building.BuildingListResultDto
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize
            });
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
