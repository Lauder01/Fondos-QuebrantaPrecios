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
    public class StatusController : ControllerBase
    {
        private readonly IRepository<Status> _statusRepository;
        private readonly IMapper _mapper;
        public StatusController(IRepository<Status> statusRepository, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StatusDto>> GetAll()
        {
            var statuses = _statusRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<StatusDto>>(statuses);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<StatusDto> Create(CreateStatusDto dto)
        {
            var status = _mapper.Map<Status>(dto);
            status.Id = Guid.NewGuid().ToString();
            _statusRepository.Add(status);
            var result = _mapper.Map<StatusDto>(status);
            return CreatedAtAction(nameof(GetAll), new { id = status.Id }, result);
        }
    }
}
