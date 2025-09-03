using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceLibraryProject;
using WebAPI.Dtos.District;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictController(DistrictService districtService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistrictGetterDto>>> GetAll()
        {
            var districts = await districtService.GetAllAsync();
            var dtos = mapper.Map<IEnumerable<DistrictGetterDto>>(districts);
            return Ok(dtos);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<DistrictGetterDto>> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("El nombre del distrito no puede estar vacío.");
            var district = await districtService.GetByNameAsync(name);
            if (district == null)
                return NotFound($"Distrito con nombre {name} no encontrado.");
            var dto = mapper.Map<DistrictGetterDto>(district);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<DistrictGetterDto>> Create(DistrictCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var district = mapper.Map<District>(dto);
            district.Id = Guid.NewGuid().ToString();
            try
            {
                await districtService.AddAsync(district);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            var result = mapper.Map<DistrictGetterDto>(district);
            return CreatedAtAction(nameof(GetAll), new { id = district.Id }, result);
        }
    }
}
