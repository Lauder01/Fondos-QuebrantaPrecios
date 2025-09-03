using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.BuildingCompany;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingCompanyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.BuildingCompanyService _companyService;
        public BuildingCompanyController(ServiceLibraryProject.BuildingCompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingCompanyGetterDto>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BuildingCompanyGetterDto>>(companies);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingCompanyGetterDto>> GetById(string id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            var dto = _mapper.Map<BuildingCompanyGetterDto>(company);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<BuildingCompanyGetterDto>> Create(BuildingCompanyCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var company = _mapper.Map<BuildingCompany>(dto);
            company.Id = Guid.NewGuid().ToString();
            await _companyService.AddAsync(company);
            var result = _mapper.Map<BuildingCompanyGetterDto>(company);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, BuildingCompanyUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var company = _mapper.Map<BuildingCompany>(dto);
            company.Id = id;
            await _companyService.UpdateAsync(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
