using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<RequestGetterDto>> GetAll()
        {
            var requests = _requestService.GetAll();
            var dtos = _mapper.Map<IEnumerable<RequestGetterDto>>(requests);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RequestGetterDto> GetById(string id)
        {
            var request = _requestService.GetById(id);
            if (request == null) return NotFound();
            var dto = _mapper.Map<RequestGetterDto>(request);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<RequestGetterDto> Create(RequestCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = _mapper.Map<Request>(dto);
            request.Id = Guid.NewGuid().ToString();
            _requestService.Add(request);
            var result = _mapper.Map<RequestGetterDto>(request);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, RequestUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = _mapper.Map<Request>(dto);
            request.Id = id;
            _requestService.Update(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _requestService.Delete(id);
            return NoContent();
        }
    }
}
