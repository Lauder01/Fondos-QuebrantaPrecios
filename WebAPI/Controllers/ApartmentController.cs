using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.Apartment;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.ApartmentService _apartmentService;
        public ApartmentController(ServiceLibraryProject.ApartmentService apartmentService, IMapper mapper)
        {
            _apartmentService = apartmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentGetterDto>>> GetAll()
        {
            var apartments = await _apartmentService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ApartmentGetterDto>>(apartments);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentGetterDto>> GetById(string id)
        {
            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment == null) return NotFound();
            var dto = _mapper.Map<ApartmentGetterDto>(apartment);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ApartmentGetterDto>> Create(ApartmentCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = Guid.NewGuid().ToString();
            await _apartmentService.AddAsync(apartment);
            var result = _mapper.Map<ApartmentGetterDto>(apartment);
            return CreatedAtAction(nameof(GetById), new { id = apartment.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ApartmentUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = id;
            await _apartmentService.UpdateAsync(apartment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _apartmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
