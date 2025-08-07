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
    public class ApartmentController : ControllerBase
    {
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IMapper _mapper;
        public ApartmentController(IRepository<Apartment> apartmentRepository, IMapper mapper)
        {
            _apartmentRepository = apartmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApartmentDto>> GetAll()
        {
            var apartments = _apartmentRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<ApartmentDto>>(apartments);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<ApartmentDto> Create(CreateApartmentDto dto)
        {
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = Guid.NewGuid().ToString();
            _apartmentRepository.Add(apartment);
            var result = _mapper.Map<ApartmentDto>(apartment);
            return CreatedAtAction(nameof(GetAll), new { id = apartment.Id }, result);
        }
    }
}
