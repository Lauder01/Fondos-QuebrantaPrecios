using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<DistrictGetterDto>> GetAll()
        {
            var districts = districtService.GetAll();
            var dtos = mapper.Map<IEnumerable<DistrictGetterDto>>(districts);
            return Ok(dtos);
        }

        [HttpGet("{Code}")]
        public ActionResult<DistrictGetterDto> GetByZipCode(string Code)
        {
            if (string.IsNullOrEmpty(Code))
                return BadRequest("El código postal no puede estar vacío.");

            var district = districtService.GetByCode(Code);
            if (district == null)
                return NotFound($"Distrito con código postal {Code} no encontrado.");

            var dto = mapper.Map<DistrictGetterDto>(district);
            return Ok(dto);
        }

        [HttpGet("name/{name}")]
        public ActionResult<DistrictGetterDto> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("El nombre del distrito no puede estar vacío.");
            var district = districtService.GetByName(name);
            if (district == null)
                return NotFound($"Distrito con nombre {name} no encontrado.");
            var dto = mapper.Map<DistrictGetterDto>(district);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<DistrictGetterDto> Create(DistrictCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var district = mapper.Map<District>(dto);
            district.Id = Guid.NewGuid().ToString();

            try
            {
                districtService.Add(district);
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

        [HttpPut("{id}")]
        public IActionResult Update(string id, DistrictUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var district = mapper.Map<District>(dto);
            district.Id = id;

            try
            {
                districtService.Update(district);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                districtService.Delete(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
