using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Dtos.Status;

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
        public ActionResult<IEnumerable<StatusBaseDto>> GetAll()
        {
            var statuses = _statusRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<StatusBaseDto>>(statuses);
            return Ok(dtos);
        }
    }
}
