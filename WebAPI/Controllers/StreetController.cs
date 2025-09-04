using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.Street;
using System;
using ServiceLibraryProject;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly StreetService _streetService;
        public StreetController(StreetService streetService, IMapper mapper)
        {
            _streetService = streetService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StreetGetterDto>>> GetAll()
        {
            var streets = await _streetService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<StreetGetterDto>>(streets);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StreetGetterDto>> GetById(string id)
        {
            var street = await _streetService.GetByIdAsync(id);
            if (street == null) return NotFound();
            var dto = _mapper.Map<StreetGetterDto>(street);
            return Ok(dto);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<StreetGetterDto>> GetByName(string name)
        {
            var street = await _streetService.GetByNameAsync(name);
            if (street == null) return NotFound();
            var dto = _mapper.Map<StreetGetterDto>(street);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<StreetGetterDto>> Create(StreetCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var street = _mapper.Map<Street>(dto);
            street.Id = Guid.NewGuid().ToString();
            await _streetService.AddAsync(street);
            var result = _mapper.Map<StreetGetterDto>(street);
            return CreatedAtAction(nameof(GetById), new { id = street.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, StreetUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var street = _mapper.Map<Street>(dto);
            street.Id = id;
            await _streetService.UpdateAsync(street);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _streetService.DeleteAsync(id);
            return NoContent();
        }
    }
}
