using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using RepositoryLibraryProject.Data;
using WebAPI.Dtos.Building;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.BuildingService _buildingService;
        private readonly ServiceLibraryProject.StatusService _statusService;
        public BuildingController(ServiceLibraryProject.BuildingService buildingService, ServiceLibraryProject.StatusService statusService, IMapper mapper)
        {
            _buildingService = buildingService;
            _statusService = statusService;
            _mapper = mapper;
        }

        [HttpGet]
        /// <summary>
        /// Obtiene una lista paginada y filtrada de edificios.
        /// </summary>
        /// <param name="page">Página actual (por defecto 1)</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
        /// <param name="name">Filtro por nombre</param>
        /// <param name="districtId">Filtro por distrito</param>
        /// <param name="companyId">Filtro por empresa constructora</param>
        /// <returns>Lista paginada de edificios</returns>
        [HttpGet("paged")]
        public ActionResult<WebAPI.Dtos.Building.BuildingListResultDto> GetPaged(
            int page = 1,
            int pageSize = 10,
            string? name = null,
            string? districtId = null,
            string? companyId = null)
        {
            var result = _buildingService.GetPagedAndFiltered(page, pageSize, name, districtId, companyId);
            var dtos = _mapper.Map<IEnumerable<BuildingGetterDto>>(result.Items);
            return Ok(new WebAPI.Dtos.Building.BuildingListResultDto
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("{id}")]
        public ActionResult<BuildingGetterDto> GetById(string id)
        {
            var building = _buildingService.GetById(id);
            if (building == null) return NotFound();
            var dto = _mapper.Map<BuildingGetterDto>(building);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<BuildingGetterDto> Create(BuildingCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Crear registros por defecto si no existen y obtener sus IDs
            var defaultIds = EnsureDefaultDataExists();
            
            var building = _mapper.Map<Building>(dto);
            building.Id = Guid.NewGuid().ToString();
            
            // Debug: Log los IDs que estamos recibiendo
            Console.WriteLine($"DistrictId recibido: '{building.DistrictId}'");
            Console.WriteLine($"StreetId recibido: '{building.StreetId}'");
            Console.WriteLine($"BuildingCompanyId recibido: '{building.BuildingCompanyId}'");
            
            // Si algún ID está vacío o no válido, usar los datos por defecto disponibles
            if (string.IsNullOrWhiteSpace(building.DistrictId) && !string.IsNullOrWhiteSpace(defaultIds.DistrictId))
            {
                building.DistrictId = defaultIds.DistrictId;
                Console.WriteLine($"Asignando distrito por defecto: {defaultIds.DistrictId}");
            }
            
            if (string.IsNullOrWhiteSpace(building.StreetId) && !string.IsNullOrWhiteSpace(defaultIds.StreetId))
            {
                building.StreetId = defaultIds.StreetId;
                Console.WriteLine($"Asignando calle por defecto: {defaultIds.StreetId}");
            }
            
            if (string.IsNullOrWhiteSpace(building.BuildingCompanyId) && !string.IsNullOrWhiteSpace(defaultIds.CompanyId))
            {
                building.BuildingCompanyId = defaultIds.CompanyId;
                Console.WriteLine($"Asignando empresa por defecto: {defaultIds.CompanyId}");
            }
            
            // Asignar status
            if (string.IsNullOrWhiteSpace(building.StatusId) && !string.IsNullOrWhiteSpace(defaultIds.StatusId))
            {
                building.StatusId = defaultIds.StatusId;
                Console.WriteLine($"Asignando status por defecto: {defaultIds.StatusId}");
            }
            
            // Generar el código usando información básica sin cargar entidades relacionadas
            building.Code = GenerateBuildingCode(building.DistrictId, building.StreetId, building.Doorway);
            Console.WriteLine($"Código generado: {building.Code}");
            
            _buildingService.Add(building);
            var result = _mapper.Map<BuildingGetterDto>(building);
            return CreatedAtAction(nameof(GetById), new { id = building.Id }, result);
        }

        private (string DistrictId, string StreetId, string CompanyId, string StatusId) EnsureDefaultDataExists()
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                
                // Obtener el primer distrito disponible (sin crear nuevos)
                var district = context.Districts.FirstOrDefault();
                string districtId = district?.Id ?? string.Empty;
                
                // Obtener la primera calle disponible (sin crear nuevos)
                var street = context.Streets.FirstOrDefault();
                string streetId = street?.Id ?? string.Empty;
                
                // Obtener la primera empresa disponible (sin crear nuevos)
                var company = context.BuildingCompanies.FirstOrDefault();
                string companyId = company?.Id ?? string.Empty;
                
                // Obtener o buscar status "Registrado" (sin crear nuevos)
                var status = context.Statuses.FirstOrDefault(s => s.Name == "Registrado") 
                           ?? context.Statuses.FirstOrDefault();
                string statusId = status?.Id ?? string.Empty;
                
                Console.WriteLine($"IDs obtenidos - Distrito: {districtId}, Calle: {streetId}, Empresa: {companyId}, Status: {statusId}");
                
                return (districtId, streetId, companyId, statusId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo datos por defecto: {ex.Message}");
                return (string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, BuildingUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var building = _mapper.Map<Building>(dto);
            building.Id = id;
            _buildingService.Update(building);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _buildingService.Delete(id);
            return NoContent();
        }

        private ClassLibraryProject.Entities.District? GetFirstAvailableDistrict()
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                return context.Districts.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private ClassLibraryProject.Entities.Street? GetFirstAvailableStreet()
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                return context.Streets.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private ClassLibraryProject.Entities.BuildingCompany? GetFirstAvailableCompany()
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                return context.BuildingCompanies.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private string GenerateBuildingCode(string districtId, string streetId, string doorway)
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                
                // Obtener el código postal del distrito
                var district = context.Districts.Include(d => d.Zipcode).FirstOrDefault(d => d.Id == districtId);
                var zipCode = district?.Zipcode?.FirstOrDefault()?.Code ?? "ZZZ";
                
                // Obtener el código de la calle
                var street = context.Streets.FirstOrDefault(s => s.Id == streetId);
                var streetCode = street?.Code ?? "STR";
                
                // Usar el doorway proporcionado
                var doorwayCode = doorway ?? "0";
                
                return $"{zipCode}-{streetCode}-{doorwayCode}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generando código: {ex.Message}");
                return $"AUTO-{Guid.NewGuid().ToString()[..8]}";
            }
        }
    }
}
