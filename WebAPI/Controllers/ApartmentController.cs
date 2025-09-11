using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Dtos.Apartment;
using System.Diagnostics;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ServiceLibraryProject.ApartmentService _apartmentService;
        private readonly ILogger<ApartmentController> _logger;
        
        public ApartmentController(ServiceLibraryProject.ApartmentService apartmentService, IMapper mapper, ILogger<ApartmentController> logger)
        {
            _apartmentService = apartmentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentGetterDto>>> GetAll()
        {
            var apartments = await _apartmentService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ApartmentGetterDto>>(apartments);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentGetterDto>> GetById(string id)
        {
            var apartment = await _apartmentService.GetByIdAsync(id);
            if (apartment == null) return NotFound();
            var dto = _mapper.Map<ApartmentGetterDto>(apartment);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ApartmentGetterDto>> Create(ApartmentCreatorDto dto)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = HttpContext.TraceIdentifier;
            
            _logger.LogInformation("Iniciando creación de apartamento - Code: {Code}, Door: {Door}, FloorId: {FloorId}, RequestId: {RequestId}",
                dto.Code, dto.Door, dto.FloorId, requestId);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validación fallida para apartamento - Errores: {ValidationErrors}, RequestId: {RequestId}",
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), requestId);
                    return BadRequest(ModelState);
                }

                var apartment = _mapper.Map<Apartment>(dto);
                apartment.Id = Guid.NewGuid().ToString();

                _logger.LogDebug("Apartamento mapeado - ApartmentId: {ApartmentId}, Code: {Code}, Door: {Door}, FloorId: {FloorId}, NumRooms: {NumRooms}, NumBathrooms: {NumBathrooms}, Area: {Area}, RequestId: {RequestId}",
                    apartment.Id, apartment.Code, apartment.Door, apartment.FloorId, apartment.NumRooms, apartment.NumBathrooms, apartment.Area, requestId);

                await _apartmentService.AddAsync(apartment);
                
                stopwatch.Stop();
                _logger.LogInformation("Apartamento creado exitosamente - ApartmentId: {ApartmentId}, Code: {Code}, Door: {Door}, FloorId: {FloorId}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    apartment.Id, apartment.Code, apartment.Door, apartment.FloorId, stopwatch.ElapsedMilliseconds, requestId);

                var result = _mapper.Map<ApartmentGetterDto>(apartment);
                return CreatedAtAction(nameof(GetById), new { id = apartment.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error de operación inválida al crear apartamento - Code: {Code}, Door: {Door}, FloorId: {FloorId}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    dto.Code, dto.Door, dto.FloorId, stopwatch.ElapsedMilliseconds, requestId);
                return BadRequest($"Error de validación: {ex.Message}");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error inesperado al crear apartamento - Code: {Code}, Door: {Door}, FloorId: {FloorId}, Duración: {ElapsedMs}ms, RequestId: {RequestId}",
                    dto.Code, dto.Door, dto.FloorId, stopwatch.ElapsedMilliseconds, requestId);
                return StatusCode(500, "Error interno del servidor al crear el apartamento");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ApartmentUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var apartment = _mapper.Map<Apartment>(dto);
            apartment.Id = id;
            await _apartmentService.UpdateAsync(apartment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _apartmentService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Env�a los datos de un apartamento a la API externa CozyHouse.
        /// </summary>
        /// <param name="id">Identificador �nico del apartamento</param>
        /// <returns>Resultado de la operaci�n</returns>
        [HttpPost("{id}/send-to-cozyhouse")]
        public async Task<IActionResult> PostToCozyHouse(string id)
        {
            try
            {
                // Buscar el apartamento por id
                var apartment = await _apartmentService.GetByIdAsync(id);
                if (apartment == null)
                {
                    // Mensaje descriptivo si no se encuentra el apartamento
                    return NotFound($"No se encontr� el apartamento con id: {id}");
                }

                // Mapear la entidad Apartment al DTO CozyHouseCreatorDto
                var dto = _mapper.Map<WebAPI.Dtos.CozyHouse.CozyHouseCreatorDto>(apartment);

                // Configura la URL de la API externa CozyHouse
                var apiUrl = "https://devdemoapi4.azurewebsites.net/api/requests"; // Cambia esto por la URL real

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(apiUrl, dto);

                // Manejo de respuesta de la API externa
                if (response.IsSuccessStatusCode)
                {
                    return Ok("DTO enviado correctamente a CozyHouse.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error al enviar a CozyHouse: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Error de red o conexi�n con la API externa
                return StatusCode(503, $"Error de conexi�n con CozyHouse: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Error inesperado en el proceso
                return StatusCode(500, $"Error interno al enviar apartamento a CozyHouse: {ex.Message}");
            }
        }
    }
}
