using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingCompanyController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BuildingCompanyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BuildingCompanyDto>> GetAll()
        {
            var companies = _context.BuildingCompanies.Select(c => new BuildingCompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Cif = c.Cif,
                Website = c.Website
            }).ToList();
            return Ok(companies);
        }

        [HttpPost]
        public ActionResult<BuildingCompanyDto> Create(CreateBuildingCompanyDto dto)
        {
            var company = new BuildingCompany
            {
                Name = dto.Name,
                Cif = dto.Cif,
                Website = dto.Website
            };
            _context.BuildingCompanies.Add(company);
            _context.SaveChanges();
            var result = new BuildingCompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Cif = company.Cif,
                Website = company.Website
            };
            return CreatedAtAction(nameof(GetAll), new { id = company.Id }, result);
        }
    }
}
