using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Dtos.Street;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreetController : ControllerBase
    {
        private readonly IRepository<Street> _streetRepository;
        private readonly IMapper _mapper;
        public StreetController(IRepository<Street> streetRepository, IMapper mapper)
        {
            _streetRepository = streetRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StreetDto>> GetAll()
        {
            var streets = _streetRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<StreetDto>>(streets);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<StreetDto> Create(CreateStreetDto dto)
        {
            var street = _mapper.Map<Street>(dto);
            street.Id = Guid.NewGuid().ToString();
            _streetRepository.Add(street);
            var result = _mapper.Map<StreetDto>(street);
            return CreatedAtAction(nameof(GetAll), new { id = street.Id }, result);
        }
    }
}
