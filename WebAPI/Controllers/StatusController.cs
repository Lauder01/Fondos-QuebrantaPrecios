using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos.Status;
using ServiceLibraryProject;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _statusService;
        private readonly IMapper _mapper;
        public StatusController(StatusService statusService, IMapper mapper)
        {
            _statusService = statusService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusBaseDto>>> GetAll()
        {
            var statuses = await _statusService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<StatusBaseDto>>(statuses);
            return Ok(dtos);
        }
    }
}
