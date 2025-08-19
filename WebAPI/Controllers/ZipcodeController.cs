using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using ServiceLibraryProject;
using WebAPI.Dtos.Zipcode;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZipcodeController(ZipcodeService zipcodeService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ZipcodeGetterDto>> GetAll()
        {
            var zipcodes = zipcodeService.GetAll();
            var dtos = mapper.Map<IEnumerable<ZipcodeGetterDto>>(zipcodes);
            return Ok(dtos);
        }

        [HttpGet("{code}")]
        public ActionResult<ZipcodeGetterDto> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("El código postal no puede estar vacío.");

            var zipcode = zipcodeService.GetByCode(code);
            if (zipcode == null)
                return NotFound($"Código postal {code} no encontrado.");

            var dto = mapper.Map<ZipcodeGetterDto>(zipcode);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<ZipcodeGetterDto> Create(ZipcodeCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var zipcode = mapper.Map<Zipcode>(dto);
            zipcode.Id = Guid.NewGuid().ToString();

            try
            {
                zipcodeService.Add(zipcode);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            var result = mapper.Map<ZipcodeGetterDto>(zipcode);
            return CreatedAtAction(nameof(GetAll), new { id = zipcode.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, ZipcodeUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var zipcode = mapper.Map<Zipcode>(dto);
            zipcode.Id = id;

            try
            {
                zipcodeService.Update(zipcode);
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
                zipcodeService.Delete(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
