using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryLibraryProject.Data;
using WebAPI.Dtos.Building;
using WebAPI.Dtos.Address;
using WebAPI.Dtos.SpecuLab;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

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
        private readonly ServiceLibraryProject.FloorService _floorService;
        private readonly ServiceLibraryProject.ApartmentService _apartmentService;
        private readonly ServiceLibraryProject.BuildingImageService _buildingImageService;
        private readonly ILogger<BuildingController> _logger;
        
        public BuildingController(
            ServiceLibraryProject.BuildingService buildingService, 
            ServiceLibraryProject.StatusService statusService, 
            ServiceLibraryProject.AddressService addressService,
            ServiceLibraryProject.FloorService floorService,
            ServiceLibraryProject.ApartmentService apartmentService,
            ServiceLibraryProject.BuildingImageService buildingImageService,
            IMapper mapper,
            ILogger<BuildingController> logger)
        {
            _buildingService = buildingService;
            _statusService = statusService;
            _addressService = addressService;
            _floorService = floorService;
            _apartmentService = apartmentService;
            _buildingImageService = buildingImageService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<WebAPI.Dtos.Building.BuildingListResultDto>> GetPaged(
            int page = 1,
            int pageSize = 10,
            string? name = null,
            string? districtId = null,
            string? companyId = null)
        {
            var result = await _buildingService.GetPagedAndFilteredAsync(page, pageSize, name, districtId, companyId);
            var dtos = _mapper.Map<IEnumerable<BuildingGetterDto>>(result.Items);

            // Optimización: Cargar todas las imágenes de los edificios de una vez
            var buildingIds = dtos.Select(dto => dto.Id).ToList();
            var allImages = await _buildingImageService.GetByBuildingIdsAsync(buildingIds);

            // Agrupar imágenes por BuildingId para consulta rápida
            var imagesByBuilding = allImages.GroupBy(img => img.BuildingId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Enriquecer los DTOs con información de imágenes
            foreach (var dto in dtos)
            {
                if (imagesByBuilding.TryGetValue(dto.Id, out var images))
                {
                    var coverImage = images.FirstOrDefault(img => img.IsCoverImage);
                    
                    dto.HasImages = images.Any();
                    if (coverImage != null)
                    {
                        dto.CoverImageId = coverImage.BuildingImageId;
                        dto.CoverImageUrl = $"/api/ImageStorage/download/{coverImage.BuildingImageId}";
                    }
                }
                else
                {
                    dto.HasImages = false;
                }
            }

            return Ok(new WebAPI.Dtos.Building.BuildingListResultDto
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingGetterDto>> GetById(string id)
        {
            var building = await _buildingService.GetByIdWithAddressAndDistrictAsync(id);
            if (building == null) return NotFound();
            var dto = _mapper.Map<BuildingGetterDto>(building);

            // Enriquecer el DTO con información de imágenes
            var images = await _buildingImageService.GetByBuildingIdAsync(dto.Id);
            var coverImage = images.FirstOrDefault(img => img.IsCoverImage);
            
            dto.HasImages = images.Any();
            if (coverImage != null)
            {
                dto.CoverImageId = coverImage.BuildingImageId;
                dto.CoverImageUrl = $"/api/ImageStorage/download/{coverImage.BuildingImageId}";
            }

            return Ok(dto);
        }

        [HttpGet("{id}/floors")]
        public ActionResult<IEnumerable<WebAPI.Dtos.Floor.FloorGetterDto>> GetFloorsByBuildingId(string id)
        {
            var building = _buildingService.GetById(id);
            if (building == null) return NotFound("Edificio no encontrado");
            var floors = _floorService.GetByBuildingId(id);
            var floorDtos = _mapper.Map<IEnumerable<WebAPI.Dtos.Floor.FloorGetterDto>>(floors);
            return Ok(floorDtos);
        }

        [HttpGet("{id}/apartments")]
        public ActionResult<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>> GetApartmentsByBuildingId(string id)
        {
            var building = _buildingService.GetById(id);
            if (building == null) return NotFound("Edificio no encontrado");
            var apartments = _apartmentService.GetByBuildingId(id);
            var apartmentDtos = _mapper.Map<IEnumerable<WebAPI.Dtos.Apartment.ApartmentGetterDto>>(apartments);
            return Ok(apartmentDtos);
        }

        [HttpPost]
        public async Task<ActionResult<BuildingGetterDto>> Create(BuildingCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var defaultIds = EnsureDefaultDataExists();
            var building = _mapper.Map<Building>(dto);
            building.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(building.DistrictId) && !string.IsNullOrWhiteSpace(defaultIds.DistrictId))
                building.DistrictId = defaultIds.DistrictId;
            if (string.IsNullOrWhiteSpace(building.StreetId) && !string.IsNullOrWhiteSpace(defaultIds.StreetId))
                building.StreetId = defaultIds.StreetId;
            if (string.IsNullOrWhiteSpace(building.BuildingCompanyId) && !string.IsNullOrWhiteSpace(defaultIds.CompanyId))
                building.BuildingCompanyId = defaultIds.CompanyId;
            if (string.IsNullOrWhiteSpace(building.StatusId) && !string.IsNullOrWhiteSpace(defaultIds.StatusId))
                building.StatusId = defaultIds.StatusId;
            building.Code = GenerateBuildingCode(building.DistrictId, building.StreetId, building.Doorway);
            await _buildingService.AddAsync(building);
            await CreateAddressForBuilding(building.Id, dto);
            CreateFloorsForBuilding(building.Id, building.FloorCount);
            CreateApartmentsForBuilding(building.Id, dto.ApartmentsPerFloor);
            var buildingWithFloors = _buildingService.GetById(building.Id);
            var result = _mapper.Map<BuildingGetterDto>(buildingWithFloors);
            return CreatedAtAction(nameof(GetById), new { id = building.Id }, result);
        }

        [HttpPost("with-images")]
        public async Task<ActionResult<BuildingGetterDto>> CreateWithImages(BuildingWithImagesCreatorDto dto)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var requestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
            var buildingId = Guid.NewGuid().ToString();

            _logger.LogInformation("Iniciando creación de edificio con imágenes - Name: {BuildingName}, Images: {ImageCount}, RequestId: {RequestId}",
                dto.Name, dto.ImageFiles?.Count ?? 0, requestId);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validación fallida para crear edificio - Errores: {ValidationErrors}, RequestId: {RequestId}",
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))), requestId);
                    return BadRequest(ModelState);
                }

                // Validar límites de imágenes
                if (dto.ImageFiles?.Count > 10)
                {
                    _logger.LogWarning("Demasiadas imágenes en request - Count: {ImageCount}, RequestId: {RequestId}",
                        dto.ImageFiles.Count, requestId);
                    return BadRequest("Máximo 10 imágenes por edificio");
                }

                _logger.LogDebug("Iniciando mapeo y configuración de datos por defecto - BuildingId: {BuildingId}, RequestId: {RequestId}",
                    buildingId, requestId);

                var defaultIds = EnsureDefaultDataExists();
                var building = _mapper.Map<Building>(dto);
                building.Id = buildingId;
                
                if (string.IsNullOrWhiteSpace(building.DistrictId) && !string.IsNullOrWhiteSpace(defaultIds.DistrictId))
                    building.DistrictId = defaultIds.DistrictId;
                if (string.IsNullOrWhiteSpace(building.StreetId) && !string.IsNullOrWhiteSpace(defaultIds.StreetId))
                    building.StreetId = defaultIds.StreetId;
                if (string.IsNullOrWhiteSpace(building.BuildingCompanyId) && !string.IsNullOrWhiteSpace(defaultIds.CompanyId))
                    building.BuildingCompanyId = defaultIds.CompanyId;
                if (string.IsNullOrWhiteSpace(building.StatusId) && !string.IsNullOrWhiteSpace(defaultIds.StatusId))
                    building.StatusId = defaultIds.StatusId;
                    
                building.Code = GenerateBuildingCode(building.DistrictId, building.StreetId, building.Doorway);

                _logger.LogDebug("Código generado para edificio: {BuildingCode}, BuildingId: {BuildingId}, RequestId: {RequestId}",
                    building.Code, buildingId, requestId);
                
                // Crear el edificio
                _logger.LogDebug("Guardando edificio en base de datos - BuildingId: {BuildingId}, RequestId: {RequestId}",
                    buildingId, requestId);
                await _buildingService.AddAsync(building);
                
                // Crear la dirección
                _logger.LogDebug("Creando dirección para edificio - BuildingId: {BuildingId}, RequestId: {RequestId}",
                    buildingId, requestId);
                await CreateAddressForBuilding(building.Id, dto);
                
                // Crear las imágenes desde los archivos base64
                if (dto.ImageFiles?.Any() == true)
                {
                    _logger.LogInformation("Procesando {ImageCount} imágenes para edificio - BuildingId: {BuildingId}, RequestId: {RequestId}",
                        dto.ImageFiles.Count, buildingId, requestId);
                    await CreateImagesFromFiles(building.Id, dto.ImageFiles);
                    _logger.LogInformation("Imágenes procesadas exitosamente - BuildingId: {BuildingId}, RequestId: {RequestId}",
                        buildingId, requestId);
                }
                
                // Crear pisos y apartamentos
                _logger.LogDebug("Creando {FloorCount} pisos y apartamentos - BuildingId: {BuildingId}, RequestId: {RequestId}",
                    building.FloorCount, buildingId, requestId);
                CreateFloorsForBuilding(building.Id, building.FloorCount);
                CreateApartmentsForBuilding(building.Id, dto.ApartmentsPerFloor);
                
                var buildingWithFloors = _buildingService.GetById(building.Id);
                var result = _mapper.Map<BuildingGetterDto>(buildingWithFloors);

                // Agregar información de imágenes
                var images = await _buildingImageService.GetByBuildingIdAsync(result.Id);
                var coverImage = images.FirstOrDefault(img => img.IsCoverImage);
                
                result.HasImages = images.Any();
                if (coverImage != null)
                {
                    result.CoverImageId = coverImage.BuildingImageId;
                    result.CoverImageUrl = $"/api/ImageStorage/download/{coverImage.BuildingImageId}";
                }

                stopwatch.Stop();

                _logger.LogInformation("Edificio creado exitosamente - BuildingId: {BuildingId}, Name: {BuildingName}, Images: {ImageCount}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    buildingId, dto.Name, dto.ImageFiles?.Count ?? 0, stopwatch.ElapsedMilliseconds, requestId);

                return CreatedAtAction(nameof(GetById), new { id = building.Id }, result);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error al crear edificio con imágenes - BuildingName: {BuildingName}, Images: {ImageCount}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    dto.Name, dto.ImageFiles?.Count ?? 0, stopwatch.ElapsedMilliseconds, requestId);
                
                throw; // Dejar que el middleware global de excepciones lo maneje
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, BuildingUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingBuilding = await _buildingService.GetByIdAsync(id);
            if (existingBuilding == null)
                return NotFound();
            var existingCode = existingBuilding.Code;
            var building = _mapper.Map<Building>(dto);
            building.Id = id;
            if (string.IsNullOrEmpty(building.Code))
                building.Code = existingCode;
            await _buildingService.UpdateAsync(building);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _buildingService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("bycode/{code}")]
        public async Task<ActionResult<SpecuLabGetterDto>> GetByCode(string code)
        {
            var building = await _buildingService.GetByCodeAsync(code);
            if (building == null) return NotFound();
            var dto = _mapper.Map<SpecuLabGetterDto>(building);
            return Ok(dto);
        }

        [HttpPatch("speculab/status/bycode/{code}")]
        public async Task<IActionResult> PatchStatusFromSpecuLab(string code, [FromBody] PatchOperationDto dto)
        {
            // Validar que el DTO no sea nulo
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío");

            // Buscar el edificio por código
            var building = await _buildingService.GetByCodeAsync(code);
            if (building == null)
                return NotFound($"No se encontró el edificio con código: {code}");

            // Validar la operación JSON Patch
            if (dto.Op?.ToLower() != "replace")
                return BadRequest("Solo se admite la operación 'replace'");

            try
            {
                // Validar y procesar el path
                var path = dto.Path?.ToLower().TrimStart('/').TrimStart('\\');
                if (path != "statusid" && path != "status" && path != "statusname")
                    return BadRequest("Solo se admiten los paths: '/StatusId', '/status' o '/statusname'");

                if (path == "statusid")
                {
                    // Si el path es StatusId, asumimos que el value es un GUID del status
                    if (!Guid.TryParse(dto.Value, out var statusGuid))
                        return BadRequest("El valor debe ser un GUID válido para StatusId");

                    // Verificar que el status existe
                    var status = await _statusService.GetByIdAsync(dto.Value);
                    if (status == null)
                        return NotFound($"No se encontró el estado con ID: {dto.Value}");

                    // Actualizar directamente con el StatusId
                    building.StatusId = dto.Value;
                    await _buildingService.UpdateAsync(building);

                    return Ok($"Status actualizado correctamente a StatusId '{dto.Value}' para el edificio con código '{code}'.");
                }
                else
                {
                    // Si el path es status o statusname, buscamos por nombre
                    if (string.IsNullOrWhiteSpace(dto.Value))
                        return BadRequest("El valor no puede estar vacío");

                    var status = await _statusService.GetByNameAsync(dto.Value);
                    if (status == null)
                        return NotFound($"No se encontró el estado con nombre: {dto.Value}");

                    // Actualizar el StatusId del edificio
                    building.StatusId = status.Id;
                    await _buildingService.UpdateAsync(building);

                    return Ok($"Status actualizado correctamente a '{dto.Value}' para el edificio con código '{code}'.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el status: {ex.Message}");
            }
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

        /// <summary>
        /// Crea automáticamente los floors para el Building recién creado
        /// </summary>
        /// <param name="buildingId">ID del edificio</param>
        /// <param name="floorCount">Número total de pisos del edificio</param>
        private void CreateFloorsForBuilding(string buildingId, int floorCount)
        {
            try
            {
                Console.WriteLine($"=== DEBUG CreateFloorsForBuilding ===");
                Console.WriteLine($"BuildingId: {buildingId}");
                Console.WriteLine($"FloorCount: {floorCount}");
                
                if (floorCount <= 0)
                {
                    Console.WriteLine("FloorCount es 0 o negativo, no se crearán floors");
                    return;
                }

                // Obtener el building para generar códigos correctos
                var building = _buildingService.GetById(buildingId);
                if (building == null)
                {
                    Console.WriteLine($"No se encontró el building con ID: {buildingId}");
                    return;
                }

                var floorsCreated = new List<string>();

                // Crear floors desde el piso 1 hasta floorCount
                // En España es común que el primer piso sea "Planta Baja" (0) y luego 1, 2, 3...
                // Pero según el dominio actual, usaremos 1, 2, 3... directamente
                for (int i = 1; i <= floorCount; i++)
                {
                    var floor = new Floor
                    {
                        Id = Guid.NewGuid().ToString(),
                        BuildingId = buildingId,
                        FloorNumber = i,
                        Building = building // Asignar la referencia del building
                    };
                    
                    // Generar el código del floor
                    floor.Code = floor.BuildFloorCode();
                    
                    Console.WriteLine($"Creando Floor #{i} con ID: {floor.Id}, Code: {floor.Code}");
                    
                    _floorService.Add(floor);
                    floorsCreated.Add($"Piso {i} (ID: {floor.Id})");
                }
                
                Console.WriteLine($"Se crearon {floorCount} floors automáticamente para Building {buildingId}:");
                foreach (var floorInfo in floorsCreated)
                {
                    Console.WriteLine($"  - {floorInfo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando floors automáticamente: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // No fallar el proceso de creación del Building por un error en los floors
            }
        }

        /// <summary>
        /// Crea automáticamente los apartamentos para el Building recién creado
        /// </summary>
        /// <param name="buildingId">ID del edificio</param>
        /// <param name="apartmentsPerFloor">Número de apartamentos por piso</param>
        private void CreateApartmentsForBuilding(string buildingId, int apartmentsPerFloor)
        {
            try
            {
                Console.WriteLine($"=== DEBUG CreateApartmentsForBuilding ===");
                Console.WriteLine($"BuildingId: {buildingId}");
                Console.WriteLine($"ApartmentsPerFloor: {apartmentsPerFloor}");
                
                if (apartmentsPerFloor <= 0)
                {
                    Console.WriteLine("ApartmentsPerFloor es 0 o negativo, no se crearán apartamentos");
                    return;
                }

                // Obtener todos los floors de este edificio
                var floors = _floorService.GetByBuildingId(buildingId);
                if (!floors.Any())
                {
                    Console.WriteLine($"No se encontraron floors para el building con ID: {buildingId}");
                    return;
                }

                var apartmentsCreated = new List<string>();

                foreach (var floor in floors)
                {
                    Console.WriteLine($"Creando {apartmentsPerFloor} apartamentos para Floor #{floor.FloorNumber} (ID: {floor.Id})");
                    
                    // Crear apartamentos para cada floor
                    for (int i = 1; i <= apartmentsPerFloor; i++)
                    {
                        var apartment = new Apartment
                        {
                            Id = Guid.NewGuid().ToString(),
                            FloorId = floor.Id,
                            Door = GenerateApartmentDoor(floor.FloorNumber, i),
                            Area = 0.0m // Área por defecto, se puede actualizar después
                        };
                        
                        // Generar el código del apartamento basado en el floor y la puerta
                        apartment.Code = GenerateApartmentCode(floor, apartment.Door);
                        
                        Console.WriteLine($"  - Apartamento {apartment.Door} (ID: {apartment.Id}, Code: {apartment.Code})");
                        
                        _apartmentService.Add(apartment);
                        apartmentsCreated.Add($"Floor {floor.FloorNumber} - Apt {apartment.Door} (ID: {apartment.Id})");
                    }
                }
                
                Console.WriteLine($"Se crearon {apartmentsCreated.Count} apartamentos automáticamente para Building {buildingId}:");
                foreach (var apartmentInfo in apartmentsCreated)
                {
                    Console.WriteLine($"  - {apartmentInfo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando apartamentos automáticamente: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // No fallar el proceso de creación del Building por un error en los apartamentos
            }
        }

        /// <summary>
        /// Genera el nombre/número de puerta para un apartamento
        /// </summary>
        /// <param name="floorNumber">Número del piso</param>
        /// <param name="apartmentNumber">Número del apartamento en el piso</param>
        /// <returns>String representando la puerta del apartamento</returns>
        private string GenerateApartmentDoor(int floorNumber, int apartmentNumber)
        {
            // Formato: {Piso}{Apartamento} (ejemplo: 1A, 1B, 2A, 2B)
            // Si hay más de 26 apartamentos por piso, usar números: 101, 102, 201, 202, etc.
            if (apartmentNumber <= 26)
            {
                char letter = (char)('A' + apartmentNumber - 1);
                return $"{floorNumber}{letter}";
            }
            else
            {
                return $"{floorNumber:D2}{apartmentNumber:D2}";
            }
        }

        /// <summary>
        /// Genera el código único para un apartamento
        /// </summary>
        /// <param name="floor">Floor al que pertenece el apartamento</param>
        /// <param name="door">Puerta del apartamento</param>
        /// <returns>Código único del apartamento</returns>
        private string GenerateApartmentCode(Floor floor, string door)
        {
            try
            {
                // Formato: {CodigoFloor}-{Puerta}
                var floorCode = floor.Code ?? $"FL{floor.FloorNumber:D2}";
                return $"{floorCode}-{door}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generando código de apartamento: {ex.Message}");
                return $"APT-{Guid.NewGuid().ToString()[..8]}-{door}";
            }
        }

        //Método para enviar el DTO a SpecuLab

        [HttpPost("{id}/send-to-speculab")]
        public async Task<IActionResult> PostToSpecuLab(string id)
        {
            var building = _buildingService.GetByIdWithAddressAndDistrict(id);
            if (building == null)
                return NotFound();

            var dto = _mapper.Map<WebAPI.Dtos.SpecuLab.SpecuLabCreatorDto>(building);
            dto.Description = string.IsNullOrWhiteSpace(dto.Description) ? "Edificio sin descripción" : dto.Description.Trim();
            dto.BuildingAmount = building.Price?.ToString() ?? "0";
            dto.MaintenanceAmount = 0; // Valor por defecto, puedes cambiarlo si tienes el dato

            // Configura la URL de la API externa
            var apiUrl = "https://devdemoapi3.azurewebsites.net/api/requests"; // URL de SpecuLab

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(apiUrl, dto);

            if (response.IsSuccessStatusCode)
                return Ok("DTO enviado correctamente a SpecuLab.");
            else
                return StatusCode((int)response.StatusCode, $"Error al enviar a SpecuLab: {await response.Content.ReadAsStringAsync()}");
        }

        /// <summary>
        /// Crea las imágenes asociadas al edificio
        /// </summary>
        /// <param name="buildingId">ID del edificio</param>
        /// <param name="imagesDtos">Lista de DTOs de imágenes a crear</param>
        private async Task CreateImagesForBuilding(string buildingId, List<WebAPI.Dtos.BuildingImage.BuildingImageCreatorDto>? imagesDtos)
        {
            if (imagesDtos == null || !imagesDtos.Any())
                return;

            foreach (var imageDto in imagesDtos)
            {
                var buildingImage = _mapper.Map<ClassLibraryProject.Entities.BuildingImage>(imageDto);
                buildingImage.BuildingImageId = Guid.NewGuid().ToString();
                buildingImage.BuildingId = buildingId;

                await _buildingImageService.AddAsync(buildingImage);
            }
        }

        /// <summary>
        /// Crea imágenes para el edificio desde archivos base64
        /// </summary>
        /// <param name="buildingId">ID del edificio</param>
        /// <param name="imageFiles">Lista de archivos de imagen en formato base64</param>
        private async Task CreateImagesFromFiles(string buildingId, List<ImageFileDto>? imageFiles)
        {
            if (imageFiles == null || !imageFiles.Any())
                return;

            // Obtener el edificio para la relación requerida
            var building = _buildingService.GetById(buildingId);
            if (building == null)
                return;

            foreach (var imageFile in imageFiles)
            {
                // Convertir base64 a bytes
                byte[] imageBytes = Convert.FromBase64String(imageFile.FileContent);
                
                var buildingImage = new ClassLibraryProject.Entities.BuildingImage
                {
                    BuildingImageId = Guid.NewGuid().ToString(),
                    BuildingId = buildingId,
                    ImageData = imageBytes,
                    FileName = imageFile.FileName ?? $"image_{DateTime.UtcNow:yyyyMMddHHmmss}",
                    Url = $"/images/buildings/{buildingId}/{imageFile.FileName}",
                    AltText = imageFile.AltText ?? "Imagen del edificio",
                    IsCoverImage = imageFile.IsCoverImage,
                    Building = building
                };
                
                await _buildingImageService.AddAsync(buildingImage);
            }
        }

    }
}
