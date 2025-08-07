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
    public class RequestController : ControllerBase
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IMapper _mapper;
        public RequestController(IRepository<Request> requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RequestDto>> GetAll()
        {
            var requests = _requestRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<RequestDto>>(requests);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<RequestDto> Create(CreateRequestDto dto)
        {
            var request = _mapper.Map<Request>(dto);
            request.Id = Guid.NewGuid().ToString();
            _requestRepository.Add(request);
            var result = _mapper.Map<RequestDto>(request);
            return CreatedAtAction(nameof(GetAll), new { id = request.Id }, result);
        }
    }
}
