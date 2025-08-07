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
    public class BuildingCompanyController : ControllerBase
    {
        private readonly IRepository<BuildingCompany> _companyRepository;
        private readonly IMapper _mapper;
        public BuildingCompanyController(IRepository<BuildingCompany> companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BuildingCompanyDto>> GetAll()
        {
            var companies = _companyRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<BuildingCompanyDto>>(companies);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<BuildingCompanyDto> Create(CreateBuildingCompanyDto dto)
        {
            var company = _mapper.Map<BuildingCompany>(dto);
            company.Id = Guid.NewGuid().ToString();
            _companyRepository.Add(company);
            var result = _mapper.Map<BuildingCompanyDto>(company);
            return CreatedAtAction(nameof(GetAll), new { id = company.Id }, result);
        }
    }
}
