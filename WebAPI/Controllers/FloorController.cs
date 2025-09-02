using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.Floor;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FloorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.FloorService _floorService;
        public FloorController(ServiceLibraryProject.FloorService floorService, IMapper mapper)
        {
            _floorService = floorService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FloorGetterDto>> GetAll()
        {
            var floors = _floorService.GetAll();
            var dtos = _mapper.Map<IEnumerable<FloorGetterDto>>(floors);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<FloorGetterDto> GetById(string id)
        {
            var floor = _floorService.GetById(id);
            if (floor == null) return NotFound();
            var dto = _mapper.Map<FloorGetterDto>(floor);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<FloorGetterDto> Create(FloorCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var floor = _mapper.Map<Floor>(dto);
            floor.Id = Guid.NewGuid().ToString();
            _floorService.Add(floor);
            var result = _mapper.Map<FloorGetterDto>(floor);
            return CreatedAtAction(nameof(GetById), new { id = floor.Id }, result);
        }
        /*
        [HttpPut("{id}")]
        public IActionResult Update(string id, FloorUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var floor = _mapper.Map<Floor>(dto);
            floor.Id = id;
            _floorService.Update(floor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _floorService.Delete(id);
            return NoContent();
        }*/

        [HttpGet("{id}/apartments")]
        public ActionResult<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>> GetApartmentsByFloorId(string id)
        {
            // Verificar que el floor existe
            var floor = _floorService.GetById(id);
            if (floor == null) return NotFound("Piso no encontrado");
            
            // Necesitamos inyectar ApartmentService
            using var scope = HttpContext.RequestServices.CreateScope();
            var apartmentService = scope.ServiceProvider.GetRequiredService<ServiceLibraryProject.ApartmentService>();
            
            // Obtener los apartamentos del piso específico
            var apartments = apartmentService.GetByFloorId(id);
            var apartmentDtos = _mapper.Map<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>>(apartments);
            
            return Ok(apartmentDtos);
        }
    }
}
