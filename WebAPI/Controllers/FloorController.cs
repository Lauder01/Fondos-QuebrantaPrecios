using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Dtos.Floor;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FloorController : ControllerBase
    {
        private readonly IRepository<Floor> _floorRepository;
        private readonly IMapper _mapper;
        public FloorController(IRepository<Floor> floorRepository, IMapper mapper)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FloorDto>> GetAll()
        {
            var floors = _floorRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<FloorDto>>(floors);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<FloorDto> Create(CreateFloorDto dto)
        {
            var floor = _mapper.Map<Floor>(dto);
            floor.Id = Guid.NewGuid().ToString();
            _floorRepository.Add(floor);
            var result = _mapper.Map<FloorDto>(floor);
            return CreatedAtAction(nameof(GetAll), new { id = floor.Id }, result);
        }
    }
}
