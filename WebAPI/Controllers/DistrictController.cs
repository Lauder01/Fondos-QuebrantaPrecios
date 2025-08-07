using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictController : ControllerBase
    {
        private readonly IRepository<District> _districtRepository;
        private readonly IMapper _mapper;
        public DistrictController(IRepository<District> districtRepository, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DistrictDto>> GetAll()
        {
            var districts = _districtRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<DistrictDto>>(districts);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<DistrictDto> Create(CreateDistrictDto dto)
        {
            var district = _mapper.Map<District>(dto);
            district.Id = Guid.NewGuid().ToString();
            _districtRepository.Add(district);
            var result = _mapper.Map<DistrictDto>(district);
            return CreatedAtAction(nameof(GetAll), new { id = district.Id }, result);
        }
    }
}
