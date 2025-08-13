using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.Address;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.AdressService _addressService;
        public AdressController(ServiceLibraryProject.AdressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AddressGetterDto>> GetAll()
        {
            var addresses = _addressService.GetAll();
            var dtos = _mapper.Map<IEnumerable<AddressGetterDto>>(addresses);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<AddressGetterDto> GetById(string id)
        {
            var address = _addressService.GetById(id);
            if (address == null) return NotFound();
            var dto = _mapper.Map<AddressGetterDto>(address);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<AddressGetterDto> Create(AddressCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var address = _mapper.Map<Address>(dto);
            address.Id = Guid.NewGuid().ToString();
            _addressService.Add(address);
            var result = _mapper.Map<AddressGetterDto>(address);
            return CreatedAtAction(nameof(GetById), new { id = address.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, AddressUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var address = _mapper.Map<Address>(dto);
            address.Id = id;
            _addressService.Update(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _addressService.Delete(id);
            return NoContent();
        }
    }
}
