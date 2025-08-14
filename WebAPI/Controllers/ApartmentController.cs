using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<ApartmentGetterDto>> GetAll()
        {
            var apartments = _apartmentService.GetAll();
            var dtos = _mapper.Map<IEnumerable<ApartmentGetterDto>>(apartments);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ApartmentGetterDto> GetById(string id)
        {
            var apartment = _apartmentService.GetById(id);
            if (apartment == null) return NotFound();
            var dto = _mapper.Map<ApartmentGetterDto>(apartment);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<ApartmentGetterDto> Create(ApartmentCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = Guid.NewGuid().ToString();
            _apartmentService.Add(apartment);
            var result = _mapper.Map<ApartmentGetterDto>(apartment);
            return CreatedAtAction(nameof(GetById), new { id = apartment.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, ApartmentUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = id;
            _apartmentService.Update(apartment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _apartmentService.Delete(id);
            return NoContent();
        }
    }
}
