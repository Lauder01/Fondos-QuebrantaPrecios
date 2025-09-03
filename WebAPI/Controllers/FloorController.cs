using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<FloorGetterDto>>> GetAll()
        {
            var floors = await _floorService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<FloorGetterDto>>(floors);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FloorGetterDto>> GetById(string id)
        {
            var floor = await _floorService.GetByIdAsync(id);
            if (floor == null) return NotFound();
            var dto = _mapper.Map<FloorGetterDto>(floor);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<FloorGetterDto>> Create(FloorCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var floor = _mapper.Map<Floor>(dto);
            floor.Id = Guid.NewGuid().ToString();
            await _floorService.AddAsync(floor);
            var result = _mapper.Map<FloorGetterDto>(floor);
            return CreatedAtAction(nameof(GetById), new { id = floor.Id }, result);
        }
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, FloorUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var floor = _mapper.Map<Floor>(dto);
            floor.Id = id;
            await _floorService.UpdateAsync(floor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _floorService.DeleteAsync(id);
            return NoContent();
        }*/

        [HttpGet("{id}/apartments")]
        public ActionResult<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>> GetApartmentsByFloorId(string id)
        {
            var floor = _floorService.GetById(id);
            if (floor == null) return NotFound("Piso no encontrado");
            using var scope = HttpContext.RequestServices.CreateScope();
            var apartmentService = scope.ServiceProvider.GetRequiredService<ServiceLibraryProject.ApartmentService>();
            var apartments = apartmentService.GetByFloorId(id);
            var apartmentDtos = _mapper.Map<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>>(apartments);
            return Ok(apartmentDtos);
        }
    }
}
