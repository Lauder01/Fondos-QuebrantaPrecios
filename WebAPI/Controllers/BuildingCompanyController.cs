using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<BuildingCompanyGetterDto>> GetAll()
        {
            var companies = _companyService.GetAll();
            var dtos = _mapper.Map<IEnumerable<BuildingCompanyGetterDto>>(companies);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BuildingCompanyGetterDto> GetById(string id)
        {
            var company = _companyService.GetById(id);
            if (company == null) return NotFound();
            var dto = _mapper.Map<BuildingCompanyGetterDto>(company);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<BuildingCompanyGetterDto> Create(BuildingCompanyCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var company = _mapper.Map<BuildingCompany>(dto);
            company.Id = Guid.NewGuid().ToString();
            _companyService.Add(company);
            var result = _mapper.Map<BuildingCompanyGetterDto>(company);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, BuildingCompanyUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var company = _mapper.Map<BuildingCompany>(dto);
            company.Id = id;
            _companyService.Update(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _companyService.Delete(id);
            return NoContent();
        }
    }
}
