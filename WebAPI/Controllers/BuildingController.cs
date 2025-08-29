using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using RepositoryLibraryProject.Data;
using WebAPI.Dtos.Building;
using WebAPI.Dtos.Address;
using WebAPI.Dtos.SpecuLab;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.BuildingService _buildingService;
        private readonly ServiceLibraryProject.StatusService _statusService;
        private readonly ServiceLibraryProject.AddressService _addressService;
        
        public BuildingController(
            ServiceLibraryProject.BuildingService buildingService, 
            ServiceLibraryProject.StatusService statusService, 
            ServiceLibraryProject.AddressService addressService,
            IMapper mapper)
        {
            _buildingService = buildingService;
            _statusService = statusService;
            _addressService = addressService;
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
        public async Task<ActionResult<BuildingGetterDto>> Create(BuildingCreatorDto dto)
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
            
            // Crear automáticamente el Address si se proporcionaron los datos necesarios
            await CreateAddressForBuilding(building.Id, dto);
            
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
                var district = context.District.FirstOrDefault();
                string districtId = district?.Id ?? string.Empty;
                
                // Obtener la primera calle disponible (sin crear nuevos)
                var street = context.Street.FirstOrDefault();
                string streetId = street?.Id ?? string.Empty;
                
                // Obtener la primera empresa disponible (sin crear nuevos)
                var company = context.BuildingCompany.FirstOrDefault();
                string companyId = company?.Id ?? string.Empty;
                
                // Obtener o buscar status "Registrado" (sin crear nuevos)
                var status = context.Status.FirstOrDefault(s => s.Name == "Registrado") 
                           ?? context.Status.FirstOrDefault();
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


        [HttpGet("bycode/{code}")]
        public ActionResult<SpecuLabGetterDto> GetByCode(string code)
        {
            var building = _buildingService.GetByCode(code);
            if (building == null) return NotFound();
            var dto = new WebAPI.Dtos.SpecuLab.SpecuLabGetterDto
            {
                BuildingCode = building.Code,
                BuildingName = building.Name ?? string.Empty,
                //building.Address?.FirstOrDefault()?.ConstructedAddress.ToString() ??
                ConstructedAddress = "N/A",
                //string.IsNullOrWhiteSpace(building.District?.Name.ToString()) ? "N/A" : building.District.Name,
                DistrictName = "N/A",
                FloorCount = building.FloorCount,
                YearBuilt = building.YearBuilt,
                ApartmentCount = building.ApartmentCount
            };
            return Ok(dto);
        }

        private ClassLibraryProject.Entities.District? GetFirstAvailableDistrict()
        {
            try
            {
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RepositoryLibraryProject.Data.AppDbContext>();
                return context.District.FirstOrDefault();
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
                return context.Street.FirstOrDefault();
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
                return context.BuildingCompany.FirstOrDefault();
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
                var district = context.District.Include(d => d.Zipcode).FirstOrDefault(d => d.Id == districtId);
                var zipCode = district?.Zipcode?.FirstOrDefault()?.Code ?? "ZZZ";
                
                // Obtener el código de la calle
                var street = context.Street.FirstOrDefault(s => s.Id == streetId);
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

        /// <summary>
        /// Crea automáticamente un Address para el Building recién creado
        /// </summary>
        private async Task CreateAddressForBuilding(string buildingId, BuildingCreatorDto dto)
        {
            try
            {
                Console.WriteLine($"=== DEBUG CreateAddressForBuilding ===");
                Console.WriteLine($"BuildingId: {buildingId}");
                Console.WriteLine($"ZipcodeId recibido: '{dto.ZipcodeId}'");
                Console.WriteLine($"ConstructedAddress: '{dto.ConstructedAddress}'");
                Console.WriteLine($"Country: '{dto.Country}'");
                Console.WriteLine($"City: '{dto.City}'");
                
                // Solo crear Address si se proporcionaron los datos mínimos necesarios
                if (string.IsNullOrWhiteSpace(dto.ZipcodeId) || 
                    string.IsNullOrWhiteSpace(dto.ConstructedAddress) ||
                    string.IsNullOrWhiteSpace(dto.Country) ||
                    string.IsNullOrWhiteSpace(dto.City))
                {
                    Console.WriteLine("Datos insuficientes para crear Address automáticamente");
                    Console.WriteLine($"ZipcodeId vacío: {string.IsNullOrWhiteSpace(dto.ZipcodeId)}");
                    Console.WriteLine($"ConstructedAddress vacío: {string.IsNullOrWhiteSpace(dto.ConstructedAddress)}");
                    Console.WriteLine($"Country vacío: {string.IsNullOrWhiteSpace(dto.Country)}");
                    Console.WriteLine($"City vacío: {string.IsNullOrWhiteSpace(dto.City)}");
                    return;
                }

                // Verificar que el ZipcodeId existe y obtener información para debug
                using var scope = HttpContext.RequestServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                // Primero, ver todos los zipcodes disponibles para debug
                var allZipcodes = await context.Zipcode.Select(z => new { z.Id, z.Code }).ToListAsync();
                Console.WriteLine($"=== Zipcodes disponibles en BD ===");
                foreach (var z in allZipcodes.Take(5)) // Solo mostrar los primeros 5
                {
                    Console.WriteLine($"ID: {z.Id}, Code: {z.Code}");
                }
                
                var zipcodeExists = await context.Zipcode.AnyAsync(z => z.Id == dto.ZipcodeId);
                Console.WriteLine($"¿Existe el zipcode con ID '{dto.ZipcodeId}'?: {zipcodeExists}");
                
                if (!zipcodeExists)
                {
                    Console.WriteLine($"No se encontró zipcode con ID: {dto.ZipcodeId}");
                    return;
                }

                // Crear la entidad Address directamente (sin AutoMapper para evitar problemas)
                var address = new Address
                {
                    Id = Guid.NewGuid().ToString(),
                    BuildingId = buildingId,
                    ApartmentId = null, // Mantener como null para direcciones de Building
                    ZipcodeId = dto.ZipcodeId,
                    ConstructedAddress = dto.ConstructedAddress,
                    IsApartment = false,
                    Country = dto.Country,
                    City = dto.City
                };

                Console.WriteLine($"=== Address entity creado directamente ===");
                Console.WriteLine($"Id: {address.Id}");
                Console.WriteLine($"BuildingId: {address.BuildingId}");
                Console.WriteLine($"ApartmentId: '{address.ApartmentId}' (is null: {address.ApartmentId == null})");
                Console.WriteLine($"ZipcodeId: {address.ZipcodeId}");
                Console.WriteLine($"IsApartment: {address.IsApartment}");

                // Agregar el Address usando el servicio
                _addressService.Add(address);
                
                Console.WriteLine($"Address creado automáticamente para Building {buildingId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando Address automáticamente: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // No fallar el proceso de creación del Building por un error en Address
            }
        }

        //Método para enviar el DTO a SpecuLab

        [HttpPost("{id}/send-to-speculab")]
        public async Task<IActionResult> PostToSpecuLab(string id)
        {
            var building = _buildingService.GetByIdWithAddressAndDistrict(id);
            if (building == null)
                return NotFound();

            var dto = new WebAPI.Dtos.SpecuLab.SpecuLabCreatorDto
            {
                BuildingCode = building.Code,
                BuildingName = building.Name ?? string.Empty,
                ConstructedAddress = building.Address?.FirstOrDefault()?.ConstructedAddress ?? "N/A",
                DistrictName = building.District?.Name ?? "N/A",
                FloorCount = building.FloorCount,
                YearBuilt = building.YearBuilt,
                ApartmentCount = building.ApartmentCount
            };

            // Configura la URL de la API externa
            var apiUrl = "https://url-de-la-otra-api/api/speculab"; // Cambia esto por la URL real

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(apiUrl, dto);

            if (response.IsSuccessStatusCode)
                return Ok("DTO enviado correctamente a SpecuLab.");
            else
                return StatusCode((int)response.StatusCode, $"Error al enviar a SpecuLab: {await response.Content.ReadAsStringAsync()}");
        }

    }
}
