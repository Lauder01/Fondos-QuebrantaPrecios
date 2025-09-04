using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.Request;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.RequestService _requestService;
        public RequestController(ServiceLibraryProject.RequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestGetterDto>>> GetAll()
        {
            var requests = await _requestService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<RequestGetterDto>>(requests);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestGetterDto>> GetById(string id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null) return NotFound();
            var dto = _mapper.Map<RequestGetterDto>(request);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<RequestGetterDto>> Create(RequestCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = _mapper.Map<Request>(dto);
            request.Id = Guid.NewGuid().ToString();
            await _requestService.AddAsync(request);
            var result = _mapper.Map<RequestGetterDto>(request);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, RequestUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = _mapper.Map<Request>(dto);
            request.Id = id;
            await _requestService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _requestService.DeleteAsync(id);
            return NoContent();
        }
    }
}
