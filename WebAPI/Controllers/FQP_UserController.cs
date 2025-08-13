using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FQP_UserController : ControllerBase
    {
        private readonly IRepository<FQP_User> _userRepository;
        private readonly IMapper _mapper;
        public FQP_UserController(IRepository<FQP_User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FQPUserDto>> GetAll()
        {
            var users = _userRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<FQPUserDto>>(users);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<FQPUserDto> Create(CreateFQPUserDto dto)
        {
            var user = _mapper.Map<FQP_User>(dto);
            user.Id = Guid.NewGuid().ToString();
            user.IsActive = true;
            // Aquí deberías hashear la contraseña si es necesario
            _userRepository.Add(user);
            var result = _mapper.Map<FQPUserDto>(user);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, result);
        }
    }
}
