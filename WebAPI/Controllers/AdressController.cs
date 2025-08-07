using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdressController : ControllerBase
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;
        public AdressController(IRepository<Address> addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AddressDto>> GetAll()
        {
            var addresses = _addressRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<AddressDto>>(addresses);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<AddressDto> Create(CreateAddressDto dto)
        {
            var address = _mapper.Map<Address>(dto);
            address.Id = Guid.NewGuid().ToString();
            _addressRepository.Add(address);
            var result = _mapper.Map<AddressDto>(address);
            return CreatedAtAction(nameof(GetAll), new { id = address.Id }, result);
        }
    }
}
