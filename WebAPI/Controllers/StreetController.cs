using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.Street;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.StreetService _streetService;
        public StreetController(ServiceLibraryProject.StreetService streetService, IMapper mapper)
        {
            _streetService = streetService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StreetGetterDto>> GetAll()
        {
            var streets = _streetService.GetAll();
            var dtos = _mapper.Map<IEnumerable<StreetGetterDto>>(streets);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<StreetGetterDto> GetById(string id)
        {
            var street = _streetService.GetById(id);
            if (street == null) return NotFound();
            var dto = _mapper.Map<StreetGetterDto>(street);
            return Ok(dto);
        }

        [HttpGet("name/{name}")]
        public ActionResult<StreetGetterDto> GetByName(string name)
        {
            var street = _streetService.GetAll().FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (street == null) return NotFound();
            var dto = _mapper.Map<StreetGetterDto>(street);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<StreetGetterDto> Create(StreetCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var street = _mapper.Map<Street>(dto);
            street.Id = Guid.NewGuid().ToString();
            _streetService.Add(street);
            var result = _mapper.Map<StreetGetterDto>(street);
            return CreatedAtAction(nameof(GetById), new { id = street.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, StreetUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var street = _mapper.Map<Street>(dto);
            street.Id = id;
            _streetService.Update(street);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _streetService.Delete(id);
            return NoContent();
        }
    }
}
